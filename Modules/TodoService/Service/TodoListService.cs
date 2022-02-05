using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TodoService.Interfaces;
using TodoService.Model;

namespace TodoService.Service;

public class TodoListService : ITodoListService
{
    private readonly TodoAppDbContext _context;

    public TodoListService(TodoAppDbContext context, IConfiguration configuration)
    {
        _context = context;
        if (!bool.TryParse(configuration["SqLite:AutomaticMigrations"], out var runMigrations) ||
            !runMigrations) return;
        context.Database.EnsureCreated();
    }

    public async Task<IEnumerable<TodoList>> GetLists()
    {
        var lists = await _context.TodoLists.ToListAsync();
        return lists;
    }

    public async Task<TodoList> GetList(string title)
    {
        var list = await _context.TodoLists.FirstOrDefaultAsync(t => t.Title.Equals(title));
        if (list == default) throw new InvalidOperationException("Todolist not found");

        return list;
    }

    public async Task<TodoList> CreateList(TodoList todoList)
    {
        if (string.IsNullOrEmpty(todoList.Title)) throw new ArgumentNullException(todoList.Title);

        await _context.TodoLists.AddAsync(todoList);
        await _context.SaveChangesAsync();
        return todoList;
    }

    public async Task<TodoList> UpdateList(TodoList todoList)
    {
        if (string.IsNullOrEmpty(todoList.Title)) throw new ArgumentNullException(todoList.Title);

        var existingList = await _context.TodoLists.FirstOrDefaultAsync(l => l.Title.Equals(todoList.Title));
        if (existingList == default) throw new InvalidOperationException("Update list not found");

        existingList = todoList;
        await _context.SaveChangesAsync();
        return existingList;
    }
}