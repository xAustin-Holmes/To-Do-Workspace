using Microsoft.EntityFrameworkCore;
using ToDoBackend.DataBridge;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ToDoDbContext")));

// adds API controller support
builder.Services.AddControllers();


var app = builder.Build();

// Forces HTTPS for security
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();


app.Run();