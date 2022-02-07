namespace ThaudalAPI.Model.Model;

public class CreateTodolistRequest
{
    public string Username { get; set; }
    public TodoList TodoList { get; set; }
}