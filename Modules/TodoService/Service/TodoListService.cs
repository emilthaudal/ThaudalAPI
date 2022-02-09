using Microsoft.Extensions.Configuration;
using ThaudalAPI.Model.Model;
using TodoService.Interfaces;
using UserService.Interfaces;

namespace TodoService.Service;

public class TodoListService : ITodoListService
{
    private readonly ThaudalDbContext _context;
    private readonly IUserService _userService;

    public TodoListService(ThaudalDbContext context, IConfiguration configuration, IUserService userService)
    {
        _context = context;
        _userService = userService;
        if (!bool.TryParse(configuration["SqLite:AutomaticMigrations"], out var runMigrations) ||
            !runMigrations) return;
        context.Database.EnsureCreated();
    }

    public async Task<IEnumerable<TodoList>> GetLists(string token)
    {
        var user = await _userService.GetFromToken(token);
        if (user == default)
        {
            throw new InvalidOperationException("User not found");
        }
        var lists = user.TodoLists;
        return lists;
    }

    public async Task<TodoList> GetList(string token, string title)
    {
        var user = await _userService.GetFromToken(token);
        var list = user.TodoLists.FirstOrDefault(t => t.Title.Equals(title));
        if (list == default) throw new InvalidOperationException("Todolist not found");

        return list;
    }

    public async Task<TodoList> CreateList(string token, TodoList todoList)
    {
        if (string.IsNullOrEmpty(todoList.Title)) throw new ArgumentNullException(todoList.Title);
        var user = await _userService.GetFromToken(token);

        user.TodoLists.Add(todoList);
        _context.Update(user);
        await _context.SaveChangesAsync();
        return todoList;
    }

    public async Task<TodoList> UpdateList(string token, TodoList todoList)
    {
        if (string.IsNullOrEmpty(todoList.Title)) throw new ArgumentNullException(todoList.Title);
        var user = await _userService.GetFromToken(token);

        var existingList = user.TodoLists.FirstOrDefault(l => l.Title.Equals(todoList.Title));
        if (existingList == default) throw new InvalidOperationException("Update list not found");

        existingList = todoList;
        _context.Update(user);
        await _context.SaveChangesAsync();
        return existingList;
    }
}