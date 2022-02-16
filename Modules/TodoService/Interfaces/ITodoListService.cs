using ThaudalAPI.Model.Model;

namespace TodoService.Interfaces;

public interface ITodoListService
{
    public Task<IEnumerable<TodoList>> GetLists(string token);
    public Task<TodoList> GetList(string token, string title);
    public Task<TodoList> CreateList(string token, TodoList todoList);
    public Task<TodoList> UpdateList(string token, TodoList todoList);
}