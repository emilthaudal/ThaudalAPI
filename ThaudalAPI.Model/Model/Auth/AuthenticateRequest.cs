using System.ComponentModel.DataAnnotations;

namespace ThaudalAPI.Model.Model.Auth;

public class AuthenticateRequest
{
    [Required] public string Username { get; set; }

    [Required] public string Password { get; set; }
}