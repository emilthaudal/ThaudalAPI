using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ThaudalAPI.Model.Model.Auth;
using ThaudalAPI.Model.Model.Users;
using UserService.Interfaces;

namespace UserService.Service;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<JwtService> _logger;

    public JwtService(ILogger<JwtService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public Task<string> GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var claims = new Dictionary<string, object>();
        if (user.Roles != null)
            foreach (var userRole in user.Roles)
                claims.Add(ClaimTypes.Role, userRole);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {new Claim("id", user.Id.ToString())}),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = _configuration["Jwt:Audience"],
            Issuer = _configuration["Jwt:Issuer"],
            Claims = claims
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(tokenHandler.WriteToken(token));
    }

    public Task<Guid> ValidateJwtToken(string? token)
    {
        if (token == null)
            return Task.FromResult<Guid>(default);

        if (token.StartsWith("Bearer")) token = token[7..];

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidIssuer = _configuration["Jwt:Issuer"]
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken) validatedToken;
            var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            return Task.FromResult(userId);
        }
        catch
        {
            return Task.FromResult<Guid>(default);
        }
    }

    public async Task<RefreshToken> GenerateRefreshToken(string ipAddress)
    {
        var refreshToken = new RefreshToken
        {
            Token = await GetUniqueToken(),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };

        return refreshToken;
    }

    private static Task<string> GetUniqueToken()
    {
        while (true)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            return Task.FromResult(token);
        }
    }
}