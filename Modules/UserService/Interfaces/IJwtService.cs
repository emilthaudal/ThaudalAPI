using ThaudalAPI.Model.Model.Auth;
using ThaudalAPI.Model.Model.Users;

namespace UserService.Interfaces;

public interface IJwtService
{
    public Task<string> GenerateJwtToken(User user);
    public Task<Guid> ValidateJwtToken(string? token);
    public Task<RefreshToken> GenerateRefreshToken(string ipAddress);
}