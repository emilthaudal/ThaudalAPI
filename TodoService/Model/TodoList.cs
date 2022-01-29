namespace TodoService.Model;

public class TodoList
{
    public TodoList(string title)
    {
        Title = title;
        TodoItems = new List<TodoItem>();
    }

    public string Title { get; set; }
    public List<TodoItem> TodoItems { get; set; }
}