using Microsoft.AspNetCore.Mvc;
using TodoAPI.Model;
using TodoAPI.Data;

namespace TodoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoDbContext _context;

        public TodoController(TodoDbContext context)
        {
            _context = context;
        }

        // GET: api/Todo
        [HttpGet]
        public IActionResult GetTodos()
        {
            var todos = _context.Todos.ToList(); // returns empty list if no data
            return Ok(todos);
        }

        // GET: api/Todo/1
        [HttpGet("{id}")]
        public IActionResult GetTodoById(int id)
        {
            var todo = _context.Todos.FirstOrDefault(t => t.id == id);
            if (todo == null) return NotFound("Todo not found");
            return Ok(todo);
        }

        // POST: api/Todo
        [HttpPost]
        public IActionResult AddTodo(Todo newTodo)
        {
            if (newTodo == null) return BadRequest("Todo cannot be null");

            _context.Todos.Add(newTodo);
            _context.SaveChanges();

            // Return the URI of the newly created resource
            return CreatedAtAction(nameof(GetTodoById), new { id = newTodo.id }, newTodo);
        }

        // PUT: api/Todo/1
        [HttpPut]
        public IActionResult UpdateTodo(Todo updatedTodo)
        {
            if (updatedTodo == null)
            {
                return BadRequest("Category cannot be null.");
            }
            _context.Todos.Update(updatedTodo);
            _context.SaveChanges();
            return Ok();
        }

        // DELETE: api/Todo/1
        [HttpDelete("{id}")]
        public IActionResult DeleteTodo(int id)
        {
            var todo = _context.Todos.FirstOrDefault(t => t.id == id);
            if (todo == null) return NotFound("Todo not found");

            _context.Todos.Remove(todo);
            _context.SaveChanges();

            return Ok("Deleted successfully");
        }
    }
}
