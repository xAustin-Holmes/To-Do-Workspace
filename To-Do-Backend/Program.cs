using Microsoft.EntityFrameworkCore;
using ToDoBackend.DataBridge;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseSqlite("Data Source=todos.db"));

// adds API controller support
builder.Services.AddControllers();


var app = builder.Build();

// Forces HTTPS for security
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();