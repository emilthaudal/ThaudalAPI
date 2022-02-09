using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaudalAPI.Model.Model;
using TodoService.Interfaces;

namespace ThaudalAPI.Controllers;

[Authorize]
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
        if (!Request.Headers.TryGetValue("Authorization", out var token)) throw new UnauthorizedAccessException();
        var lists = await _service.GetLists(token);
        foreach (var todoList in lists) yield return todoList;
    }

    [HttpPost]
    public async Task<TodoList> CreateList([FromBody] TodoList todoList)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var token)) throw new UnauthorizedAccessException();

        return await _service.CreateList(token, todoList);
    }
}