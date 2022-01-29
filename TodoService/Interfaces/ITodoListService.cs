using TodoService.Model;

namespace TodoService.Interfaces;

public interface ITodoListService
{
    public IQueryable<TodoList> GetLists();
    public Task<TodoList> GetList(string title);
}