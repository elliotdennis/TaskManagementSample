using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Application.Interfaces;
using TaskManagementAPI.Application.Services;
using TaskManagementAPI.DataAccess.Interfaces;
using TaskManagementAPI.DataAccess.Repositories;
using TaskManagementAPI.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// set up faked db
builder.Services.AddDbContext<TaskManagementContext>(options => options.UseInMemoryDatabase("TaskManagement"));

// Add services
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();
