using Microsoft.EntityFrameworkCore;
using ThaudalAPI.Helpers;
using TodoService.Interfaces;
using TodoService.Model;
using TodoService.Service;
using UserService.Authorization;
using UserService.Interfaces;
using UserService.Service;

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
            corsBuilder => { corsBuilder.WithOrigins("localhost"); });
    });
}
else
{
    builder.Services.AddDbContextFactory<TodoAppDbContext>(options => options.UseCosmos(
        builder.Configuration["Cosmos:ConnectionString"],
        builder.Configuration["Cosmos:Database"]));
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            corsBuilder => { corsBuilder.WithOrigins("https://thaudal.azurewebsites.net"); });
    });
}


builder.Services.AddScoped<ITodoListService, TodoListService>();
builder.Services.AddScoped<IUserService, UserService.Service.UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();