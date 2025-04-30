using Microsoft.EntityFrameworkCore;
using ToDoBackend.Models;


namespace ToDoBackend.DataBridge
{
    // create class ToDoDbContext based on DbContext
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options) 
        {
        }

        // creates table of ToDo items in database
        public DbSet<ToDoItem> ToDoItems { get; set; } = null!;
    }
}
