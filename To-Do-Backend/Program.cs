using Microsoft.EntityFrameworkCore;
using ToDoBackend.DataBridge;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseSqlite("Data Source=todos.db"));

// Adds CORS for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// adds API controller support
builder.Services.AddControllers();


var app = builder.Build();

// Forces HTTPS for security
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowFrontend");

app.Run();