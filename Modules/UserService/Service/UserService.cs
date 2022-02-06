using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UserService.Interfaces;
using UserService.Model;
using UserService.Model.Auth;
using UserService.Model.Users;

namespace UserService.Service;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;
    private readonly UserDataContext _context;
    private readonly IJwtService _jwtService;
    private readonly ILogger<UserService> _logger;

    public UserService(ILogger<UserService> logger,
        IJwtService jwtService,
        IConfiguration configuration, UserDataContext context)
    {
        _logger = logger;
        _jwtService = jwtService;
        _configuration = configuration;
        _context = context;
        
        if (!bool.TryParse(configuration["SqLite:AutomaticMigrations"], out var runMigrations) ||
            !runMigrations) return;
        context.Database.EnsureCreated();
    }

    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
    {
        var user = _context.Users.SingleOrDefault(x => x.Username == model.Username);

        // Validate the user
        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            throw new InvalidOperationException("Username or password is incorrect");
        _logger.LogInformation("User {UserId} valid. Generating tokens", user.Id);

        // Authentication ok, generate tokens
        var jwtToken = await _jwtService.GenerateJwtToken(user);
        var refreshToken = await _jwtService.GenerateRefreshToken(ipAddress);
        user.RefreshTokens.Add(refreshToken);

        // Remove old refresh tokens
        RemoveOldRefreshTokens(user);

        // Save updated refresh tokens
        _context.Update(user);
        await _context.SaveChangesAsync();

        return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
    }

    public async Task<AuthenticateResponse> RefreshToken(string token, string ipAddress)
    {
        var user = GetUserByRefreshToken(token);
        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (refreshToken.IsRevoked)
        {
            RevokeDescendantRefreshTokens(refreshToken, user, ipAddress,
                $"Attempted reuse of revoked ancestor token: {token}");
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        if (!refreshToken.IsActive)
            throw new InvalidOperationException("Invalid token");

        var newRefreshToken = await RotateRefreshToken(refreshToken, ipAddress);
        user.RefreshTokens.Add(newRefreshToken);

        RemoveOldRefreshTokens(user);

        _context.Update(user);
        await _context.SaveChangesAsync();

        var jwtToken = await _jwtService.GenerateJwtToken(user);

        return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
    }

    public async Task RevokeToken(string token, string ipAddress)
    {
        var user = GetUserByRefreshToken(token);
        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (!refreshToken.IsActive)
            throw new InvalidOperationException("Invalid token");

        RevokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
        _context.Update(user);
        await _context.SaveChangesAsync();
    }

    public Task<IEnumerable<User>> GetAll()
    {
        return Task.FromResult(_context.Users.AsEnumerable());
    }

    public async Task<User> GetById(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }

    public async Task<CreateUserResponse> CreateUser(CreateUserRequest createUserRequest)
    {
        var response = new CreateUserResponse();
        if (string.IsNullOrEmpty(createUserRequest.UserName))
        {
            response.Result = CreateUserResult.InvalidEmail;
            return response;
        }

        try
        {
            createUserRequest.UserName = new MailAddress(createUserRequest.UserName).Address;
        }
        catch (FormatException)
        {
            response.Result = CreateUserResult.InvalidEmail;
            return response;
        }

        if (string.IsNullOrEmpty(createUserRequest.Password))
        {
            response.Result = CreateUserResult.InvalidPassword;
            return response;
        }

        if (string.IsNullOrEmpty(createUserRequest.Name))
        {
            response.Result = CreateUserResult.InvalidName;
            return response;
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(createUserRequest.UserName));
        if (user != default)
        {
            response.Result = CreateUserResult.ExistingUser;
            return response;
        }

        user = new User
        {
            Name = createUserRequest.Name,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserRequest.Password),
            Username = createUserRequest.UserName,
            Roles = new List<UserRole> { new UserRole() {Role = "User"}}
        };
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
        response.Result = CreateUserResult.Ok;
        response.User = user;
        return response;
    }

    public async Task<bool> ValidateEmail(Guid userId)
    {
        var user = await GetById(userId);
        user.EmailValidated = true;
        _context.Update(user);
        await _context.SaveChangesAsync();
        return true;
    }

    private User GetUserByRefreshToken(string token)
    {
        var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

        if (user == null)
            throw new InvalidOperationException("Invalid token");

        return user;
    }

    private async Task<RefreshToken> RotateRefreshToken(RefreshToken refreshToken, string ipAddress)
    {
        var newRefreshToken = await _jwtService.GenerateRefreshToken(ipAddress);
        RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }

    private void RemoveOldRefreshTokens(User user)
    {
        var tokenLifeTime = int.Parse(_configuration["jwt:RefreshTokenTTL"]);
        if (tokenLifeTime == default) return;
        user.RefreshTokens.RemoveAll(x =>
            !x.IsActive &&
            x.Created.AddDays(tokenLifeTime) <= DateTime.UtcNow);
    }

    private static void RevokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress,
        string reason)
    {
        if (string.IsNullOrEmpty(refreshToken.ReplacedByToken)) return;
        var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
        if (childToken == null) return;
        if (childToken.IsActive)
            RevokeRefreshToken(childToken, ipAddress, reason);
        else
            RevokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
    }

    private static void RevokeRefreshToken(RefreshToken token, string ipAddress, string? reason = null,
        string? replacedByToken = null)
    {
        token.Revoked = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        token.ReasonRevoked = reason;
        token.ReplacedByToken = replacedByToken;
    }

    private MailMessage SendValidationMessage(User createdUser)
    {
        var host = _configuration["Mail:SmtpHost"];
        if (string.IsNullOrEmpty(host))
        {
            throw new InvalidOperationException("SMTP Not configured correctly");
        }
        var port = _configuration["Mail:SmtpPort"];
        if (string.IsNullOrEmpty(port) || !int.TryParse(port, out var portNumber))
        {
            throw new InvalidOperationException("SMTP Not configured correctly");
        }

        var username = _configuration["Mail:Username"];
        var password = _configuration["Mail:Password"];

        if (string.IsNullOrEmpty(username) ||string.IsNullOrEmpty(password))
        {
            throw new InvalidOperationException("SMTP Not configured correctly");
        }
        var client = new SmtpClient(host, portNumber);

        client.Credentials = new NetworkCredential(username, password);
        client.EnableSsl = true;
        var confirmUrl = $"https://localhost:7209/api/users/confirm/{createdUser.Id}";

        var mailMessage = new MailMessage
        {
            Body = "Confirm account:" + Environment.NewLine + confirmUrl
        };
        client.Send(mailMessage);
        return mailMessage;
    }
}