using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TodoService.Interfaces;
using TodoService.Model;

namespace TodoService.Service;

public class TodoListService: ITodoListService
{
    private readonly IDbContextFactory<TodoAppDbContext> _dbContextFactory;

    public TodoListService(IDbContextFactory<TodoAppDbContext> dbContextFactory, IConfiguration configuration)
    {
        _dbContextFactory = dbContextFactory;
        if (!bool.TryParse(configuration["SqLite:AutomaticMigrations"], out var runMigrations) ||
            !runMigrations) return;
        using var context = _dbContextFactory.CreateDbContext();
        context.Database.EnsureCreated();
    }
    public async Task<IEnumerable<TodoList>> GetLists()
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();
        var lists = context.TodoLists.ToList();
        return lists;
    }

    public async Task<TodoList> GetList(string title)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();
        var list = await context.TodoLists.FirstOrDefaultAsync(t => t.Title.Equals(title));
        if (list == default)
        {
            throw new InvalidOperationException("Todolist not found");
        }

        return list;
    }

    public async Task<TodoList> CreateList(TodoList todoList)
    {
        if (string.IsNullOrEmpty(todoList.Title))
        {
            throw new ArgumentNullException(todoList.Title);
        }

        await using var context = await _dbContextFactory.CreateDbContextAsync();
        await context.TodoLists.AddAsync(todoList);
        await context.SaveChangesAsync();
        return todoList;
    }

    public async Task<TodoList> UpdateList(TodoList todoList)
    {
        if (string.IsNullOrEmpty(todoList.Title))
        {
            throw new ArgumentNullException(todoList.Title);
        }

        await using var context = await _dbContextFactory.CreateDbContextAsync();
        var existingList = await context.TodoLists.FirstOrDefaultAsync(l => l.Title.Equals(todoList.Title));
        if (existingList == default)
        {
            throw new InvalidOperationException("Update list not found");
        }

        existingList = todoList;
        await context.SaveChangesAsync();
        return existingList;
    }
}