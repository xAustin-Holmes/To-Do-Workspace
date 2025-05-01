using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoBackend.DataBridge; // 👈 your DbContext namespace
using ToDoBackend.Models; // 👈 your ToDoItem model namespace

namespace ToDoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ToDoDbContext _context;

        public TodoController(ToDoDbContext context)
        {
            _context = context;
        }

        // GET: api/ToDoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetTodos()
        {
            return await _context.ToDoItems.ToListAsync();
        }

        // GET: api/ToDoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> GetTodo(int id)
        {
            var todo = await _context.ToDoItems.FindAsync(id);
            if (todo == null) return NotFound();
            return todo;
        }

        // POST: api/ToDoItems
        [HttpPost]
        public async Task<ActionResult<ToDoItem>> PostTodo(ToDoItem todo)
        {
            _context.ToDoItems.Add(todo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        // PUT: api/ToDoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodo(int id, ToDoItem todo)
        {
            if (id != todo.Id) return BadRequest();

            _context.Entry(todo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ToDoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var todo = await _context.ToDoItems.FindAsync(id);
            if (todo == null) return NotFound();

            _context.ToDoItems.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
