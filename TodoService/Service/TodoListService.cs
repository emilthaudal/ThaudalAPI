using Microsoft.EntityFrameworkCore;
using TodoService.Interfaces;
using TodoService.Model;

namespace TodoService.Service;

public class TodoListService: ITodoListService
{
    private readonly IDbContextFactory<TodoAppDbContext> _dbContextFactory;

    public TodoListService(IDbContextFactory<TodoAppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    public IQueryable<TodoList> GetLists()
    {
        using var context = _dbContextFactory.CreateDbContext();
        return context.TodoLists.AsQueryable();
    }

    public async Task<TodoList> GetList(string title)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();
        return await context.TodoLists.FirstOrDefaultAsync(t => t.Title.Equals(title));
    }
}