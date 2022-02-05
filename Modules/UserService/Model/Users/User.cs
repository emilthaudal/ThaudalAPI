using System.Text.Json.Serialization;
using UserService.Model.Auth;

namespace UserService.Model.Users;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public bool EmailValidated { get; set; }
    public string? Role { get; set; }

    [JsonIgnore] public string PasswordHash { get; set; }

    [JsonIgnore] public List<RefreshToken> RefreshTokens { get; set; }
}