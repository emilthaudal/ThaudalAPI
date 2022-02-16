using ThaudalAPI.Model.Model.Auth;
using ThaudalAPI.Model.Model.Users;

namespace UserService.Interfaces;

public interface IUserService
{
    Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress);
    Task<AuthenticateResponse> RefreshToken(string token, string ipAddress);
    Task RevokeToken(string token, string ipAddress);
    IAsyncEnumerable<User> GetAll();
    Task<User> GetById(string id);
    Task<User?> GetFromToken(string token);
    Task<CreateUserResponse> CreateUser(CreateUserRequest createUserRequest);
    Task<User> UpdateUser(User user);
    Task<bool> ValidateEmail(string userId);
}