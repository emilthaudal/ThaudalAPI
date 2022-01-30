using TodoService.Model;

namespace TodoService.Interfaces;

public interface ITodoListService
{
    public Task<IEnumerable<TodoList>> GetLists();
    public Task<TodoList> GetList(string title);
    public Task<TodoList> CreateList(TodoList todoList);
    public Task<TodoList> UpdateList(TodoList todoList);
}