namespace ThaudalAPI.Model.Model;

public class TodoItem
{
    public TodoItem(bool completed, string title)
    {
        Completed = completed;
        Title = title;
        Priority = 2;
    }

    public Guid Id { get; set; }
    public bool Completed { get; set; }
    public string Title { get; set; }
    public DateTimeOffset? CompletedTime { get; set; }
    public DateTimeOffset? DueDate { get; set; }
    public string? Description { get; set; }
    public int Priority { get; set; }
    public List<TodoItem>? SubTasks { get; set; }
}