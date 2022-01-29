using Microsoft.EntityFrameworkCore;

namespace TodoService.Model;

public class TodoAppDbContext: DbContext
{
    public DbSet<TodoList> TodoLists { get; set; }
    public DbSet<TodoItem> TodoItems { get; set; }
}