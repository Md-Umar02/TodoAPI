using Microsoft.AspNetCore.Mvc;
using TodoAPI.Model;
using TodoAPI.Data;
using TodoAPI.DTOs;
using TodoAPI.Repositories;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace TodoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository  _repository;

        public TodoController(ITodoRepository  repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _repository.GetAllAsync();
            var todoDto = todos
                .Select(t => new TodoDto
                {
                    id = t.id,
                    Title = t.Title,
                    IsCompleted = t.IsCompleted,
                    StatusMessage = t.IsCompleted ? "Done ✅" : "Pending ⏳"
                })
                .ToList();

            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var todo = await _repository.GetByIdAsync(id);
            if (todo == null) return NotFound();

            var todoDto = new TodoDto
            {
                id = todo.id,
                Title = todo.Title,
                IsCompleted = todo.IsCompleted,
                StatusMessage = todo.IsCompleted ? "Done ✅" : "Pending ⏳"
            };

            return Ok(todoDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTodoDto todoDto)
        {
            var todo = new Todo
            {
                Title = todoDto.Title,
                IsCompleted = todoDto.IsCompleted
            };

            await _repository.AddAsync(todo);

            return CreatedAtAction(nameof(GetById), new { id = todo.id }, todoDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateTodoDto todoDto)
        {
            var todo = await _repository.GetByIdAsync(id);
            if (todo == null) return NotFound();
            todo.Title = todoDto.Title;
            todo.IsCompleted = todoDto.IsCompleted;

            await _repository.UpdateAsync(todo);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var todoDto = await _repository.GetByIdAsync(id);
            if (todoDto == null) return NotFound();

            await _repository.DeleteAsync(id);

            return NoContent();
        }
    }
}
