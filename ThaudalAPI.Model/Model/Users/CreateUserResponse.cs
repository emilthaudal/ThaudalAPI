namespace ThaudalAPI.Model.Model.Users;

public enum CreateUserResult
{
    Ok,
    InvalidEmail,
    ExistingUser,
    InvalidPassword,
    InvalidName
}

public class CreateUserResponse
{
    public User? User { get; set; }
    public CreateUserResult Result { get; set; }
    public string GetResult => Result.ToString();
}