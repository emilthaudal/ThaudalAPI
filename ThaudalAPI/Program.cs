using System.Text;
using DayOfWeekService.Interfaces;
using DayOfWeekService.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ThaudalAPI.Helpers;
using TodoService.Interfaces;
using TodoService.Model;
using TodoService.Service;
using UserService.Interfaces;
using UserService.Model;
using UserService.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", // must be lower case
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, Array.Empty<string>()}
    });
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<TodoAppDbContext>(options =>
        options.UseSqlite(builder.Configuration["SqLite:ConnectionString"]));
    builder.Services.AddDbContext<UserDataContext>(options =>
        options.UseSqlite(builder.Configuration["SqLite:ConnectionString"]));
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            corsBuilder => { corsBuilder.WithOrigins("https://localhost:3000", "http://localhost:3000", "localhost"); });
    });
}
else
{
    builder.Services.AddDbContext<TodoAppDbContext>(options => options.UseCosmos(
        builder.Configuration["Cosmos:ConnectionString"],
        builder.Configuration["Cosmos:Database"]));
    
    builder.Services.AddDbContext<UserDataContext>(options => options.UseCosmos(
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
builder.Services.AddScoped<IDateService, DateService>();
builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    };
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();