using ThaudalAPI.Model.Model.Users;

namespace ThaudalAPI.Model.Model.Auth;

public class AuthenticateResponse
{
    public AuthenticateResponse(User user, string jwtToken, string refreshToken)
    {
        Id = user.Id;
        Name = user.Name;
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public string JwtToken { get; set; }
    public string RefreshToken { get; set; }
}