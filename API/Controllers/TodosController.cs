using API.DTOs;
using API.Entities;
using API.Services.Interface;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers
{
    public class TodosController : BaseApiController
    {
        private readonly ITodosService _todosService;
        public TodosController(ITodosService todosService)
        {
            this._todosService = todosService;

        }

        [HttpGet]
        public async Task<ActionResult> GetTodoItems()
        {
            var response = await _todosService.GetAllTodoItems();
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        {
            var response = await _todosService.GetTodoItemById(id);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost("Add")]
        public async Task<ActionResult> PostTodoItem(TodoItemAddDto todoAddDto)
        {
            var response = await _todosService.AddTodoItem(todoAddDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> PutTodoItem(TodoItemUpdateDto todoItemDto)
        {
            var response = await _todosService.UpdateTodoItem(todoItemDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var response = await _todosService.DeleteTodoItem(id);

            return StatusCode(response.StatusCode, response);
        }


    }
}