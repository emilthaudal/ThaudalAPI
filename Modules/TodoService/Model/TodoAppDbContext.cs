using Microsoft.EntityFrameworkCore;

namespace TodoService.Model;

public class TodoAppDbContext : DbContext
{
    public TodoAppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<TodoList> TodoLists { get; set; }
}