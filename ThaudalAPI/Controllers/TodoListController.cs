using Microsoft.AspNetCore.Mvc;
using TodoService.Interfaces;
using TodoService.Model;

namespace ThaudalAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TodoListController : ControllerBase
{
    private readonly ITodoListService _service;

    public TodoListController(ITodoListService service)
    {
        _service = service;
    }

    [HttpGet]
    public async IAsyncEnumerable<TodoList> GetLists()
    {
        var lists = await _service.GetLists();
        foreach (var todoList in lists) yield return todoList;
    }

    [HttpPost]
    public async Task<TodoList> CreateList([FromBody] TodoList todoList)
    {
        return await _service.CreateList(todoList);
    }
}