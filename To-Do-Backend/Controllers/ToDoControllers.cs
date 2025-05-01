using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoBackend.DataBridge;
using ToDoBackend.Models;

namespace ToDoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoDbContext _context;

        public ToDoController(ToDoDbContext context)
        {
            _context = context;
        }

        // GET: api/ToDoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItemDTO>>> GetToDos()
        {
            return await _context.ToDoItems
                .Select(item => ItemToDTO(item))
                .ToListAsync();
        }

        // GET: api/ToDoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItemDTO>> GetToDo(int id)
        {
            var todo = await _context.ToDoItems.FindAsync(id);
            if (todo == null) return NotFound();
            return ItemToDTO(todo);
        }

        // POST: api/ToDoItems
        [HttpPost]
        public async Task<ActionResult<ToDoItemDTO>> PostToDo(ToDoItemDTO todoDTO)
        {   
            var todo = new ToDoItem
            {
                Title = todoDTO.Title,
                IsUrgent = todoDTO.IsUrgent,
                IsCompleted = todoDTO.IsCompleted,
                CreatedAt = DateTime.UtcNow
            };

            _context.ToDoItems.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetToDo), new { id = todo.Id }, ItemToDTO(todo));
        }

        // PUT: api/ToDoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDo(int id, ToDoItemDTO todoDTO)
        {
            if (id != todoDTO.Id) return BadRequest();

            var todo = await _context.ToDoItems.FindAsync(id);

            if (todo == null) return NotFound();

            todo.Title = todoDTO.Title;
            todo.IsUrgent = todoDTO.IsUrgent;
            todo.IsCompleted = todoDTO.IsCompleted;
            todo.CreatedAt = DateTime.UtcNow; // Update CreatedAt time to current time   

            _context.Entry(todo).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ToDoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDo(int id)
        {
            var todo = await _context.ToDoItems.FindAsync(id);

            if (todo == null) return NotFound();

            _context.ToDoItems.Remove(todo);
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // item to DTO
        private static ToDoItemDTO ItemToDTO(ToDoItem item) =>
            new ToDoItemDTO
            {
                Id = item.Id,
                Title = item.Title,
                IsUrgent = item.IsUrgent,
                IsCompleted = item.IsCompleted,
                CreatedAt = item.CreatedAt
            };
    }
}
