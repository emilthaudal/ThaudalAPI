using System.Text.Json.Serialization;
using ThaudalAPI.Model.Model.Auth;

namespace ThaudalAPI.Model.Model.Users;

public class User
{
    public User()
    {
        RefreshTokens = new List<RefreshToken>();
        TodoLists = new List<TodoList>();
    }
    public User(string name, string username, bool emailValidated, string passwordHash)
    {
        Id = Guid.NewGuid();
        Name = name;
        Username = username;
        EmailValidated = emailValidated;
        PasswordHash = passwordHash;
        RefreshTokens = new List<RefreshToken>();
        TodoLists = new List<TodoList>();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set;  }
    public bool EmailValidated { get; set; }
    public List<UserRole>? Roles { get; set; }

    [JsonIgnore] public string PasswordHash { get; set; }

    [JsonIgnore] public List<RefreshToken> RefreshTokens { get; set; }
    
    // Todolists
    public List<TodoList> TodoLists { get; set; }
}