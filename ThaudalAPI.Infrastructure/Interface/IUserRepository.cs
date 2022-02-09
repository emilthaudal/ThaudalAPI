using ThaudalAPI.Model.Model.Users;

namespace ThaudalAPI.Infrastructure.Interface;

public interface IUserRepository
{
    public Task<User?> GetUserByIdAsync(Guid id);
    public Task<User?> GetUserByUsername(string username);
    public Task<User?> GetUserByRefreshToken(string refreshToken);
    public IAsyncEnumerable<User> GetUsersAsync(string? queryString, Dictionary<string, object> parameters);
    public IAsyncEnumerable<User> GetUsersAsync(string? queryString);

    public Task<User?> CreateUserAsync(User user);
    public Task<User?> UpdateUserAsync(User user);
}