using Microsoft.EntityFrameworkCore;
using TodoService.Interfaces;
using TodoService.Model;
using TodoService.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContextFactory<TodoAppDbContext>(options =>
        options.UseSqlite(builder.Configuration["SqLite:ConnectionString"]));
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            cBuilder =>
            {
                cBuilder.WithOrigins("http://localhost:3000",
                    "https://localhost:3000");
            });
    });
}
else
    builder.Services.AddDbContextFactory<TodoAppDbContext>(options => options.UseCosmos(
        builder.Configuration["Cosmos:ConnectionString"],
        builder.Configuration["Cosmos:Database"]));

builder.Services.AddScoped<ITodoListService, TodoListService>();
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();