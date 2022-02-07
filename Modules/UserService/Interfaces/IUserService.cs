using ThaudalAPI.Model.Model.Auth;
using ThaudalAPI.Model.Model.Users;

namespace UserService.Interfaces;

public interface IUserService
{
    Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress);
    Task<AuthenticateResponse> RefreshToken(string token, string ipAddress);
    Task RevokeToken(string token, string ipAddress);
    Task<IEnumerable<User>> GetAll();
    Task<User> GetById(Guid id);
    Task<User> GetByUsername(string username);
    Task<User?> GetFromToken(string token);
    Task<CreateUserResponse> CreateUser(CreateUserRequest createUserRequest);
    Task<bool> ValidateEmail(Guid userId);
}