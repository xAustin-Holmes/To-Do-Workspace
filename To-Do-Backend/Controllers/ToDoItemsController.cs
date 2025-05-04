using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoBackend.DataBridge;
using ToDoBackend.Models;

namespace ToDoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoItemsController : ControllerBase
    {
        private readonly ToDoDbContext _context;

        public ToDoItemsController(ToDoDbContext context)
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
            Console.WriteLine($"Received todo: {todoDTO.title}, {todoDTO.isUrgent}, {todoDTO.isCompleted}, {todoDTO.createdAt}");

            var todo = new ToDoItem
            {
                title = todoDTO.title,
                isUrgent = todoDTO.isUrgent,
                isCompleted = todoDTO.isCompleted,
                createdAt = DateTime.UtcNow
            };

            _context.ToDoItems.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetToDo), new { id = todo.id }, ItemToDTO(todo));
        }

        // PUT: api/ToDoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDo(int id, ToDoItemDTO todoDTO)
        {
            if (id != todoDTO.id) return BadRequest();

            var todo = await _context.ToDoItems.FindAsync(id);

            if (todo == null) return NotFound();

            todo.title = todoDTO.title;
            todo.isUrgent = todoDTO.isUrgent;
            todo.isCompleted = todoDTO.isCompleted;
            todo.createdAt = DateTime.UtcNow; // Update CreatedAt time to current time   

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
                id = item.id,
                title = item.title,
                isUrgent = item.isUrgent,
                isCompleted = item.isCompleted,
                createdAt = item.createdAt
            };
    }
}
