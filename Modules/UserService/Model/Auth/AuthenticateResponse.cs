using System.Text.Json.Serialization;
using UserService.Model.Users;

namespace UserService.Model.Auth;

public class AuthenticateResponse
{
    public AuthenticateResponse(User user, string jwtToken, string refreshToken)
    {
        Id = user.Id;
        Name = user.Name;
        Username = user.Username;
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string JwtToken { get; set; }
    public string RefreshToken { get; set; }
}