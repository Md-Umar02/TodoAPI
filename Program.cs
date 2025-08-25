using Microsoft.EntityFrameworkCore;
using TodoAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDbContext>(options =>
options.UseSqlServer(builder.Configuration
.GetConnectionString("DefaultConnection")));

// 1️⃣ Add services
builder.Services.AddControllers();           // Enables controllers
builder.Services.AddEndpointsApiExplorer(); // Needed for Swagger
builder.Services.AddSwaggerGen();           // Swagger UI

var app = builder.Build();

// 2️⃣ Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Optional HTTPS redirection
// app.UseHttpsRedirection();

app.UseAuthorization();

// 3️⃣ Map controllers
app.MapControllers(); // MUST have this to connect controller routes

// 4️⃣ Run the app
app.Run();
