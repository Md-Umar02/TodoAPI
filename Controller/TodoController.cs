using Microsoft.AspNetCore.Mvc;
using TodoAPI.Model;
using TodoAPI.Data;
using TodoAPI.DTOs;
using Microsoft.AspNetCore.Components.Web;

namespace TodoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoDbContext  _context;

        public TodoController(TodoDbContext  context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var todos = _context.Todos
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
        public IActionResult GetById(int id)
        {
            var todo = _context.Todos.Find(id);
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
        public IActionResult Create(CreateTodoDto todoDto)
        {
            var todo = new Todo
            {
                Title = todoDto.Title,
                IsCompleted = todoDto.IsCompleted
            };

            _context.Todos.Add(todo);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = todo.id }, todoDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateTodoDto todoDto)
        {
            var todo = _context.Todos.Find(id);
            if (todo == null) return NotFound();
            todo.Title = todoDto.Title;
            todo.IsCompleted = todoDto.IsCompleted;

            _context.Todos.Update(todo);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var todo = _context.Todos.Find(id);
            if (todo == null) return NotFound();

            _context.Todos.Remove(todo);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
