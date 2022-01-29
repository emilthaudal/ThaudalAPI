using Microsoft.AspNetCore.Mvc;
using TodoService.Interfaces;
using TodoService.Model;

namespace ThaudalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        private readonly ITodoListService _service;

        public TodoListController(ITodoListService service)
        {
            _service = service;
        }
        public async IAsyncEnumerable<TodoList> GetLists()
        {
            var lists = _service.GetLists();
            foreach (var todoList in lists)
            {
                yield return todoList;
            }
        }
    }
}