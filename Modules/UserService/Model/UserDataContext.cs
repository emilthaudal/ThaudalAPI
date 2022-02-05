using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserService.Model.Users;

namespace UserService.Model;

public class UserDataContext : DbContext
{
    private readonly IConfiguration _configuration;

    public UserDataContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // in memory database used for simplicity, change to a real db for production applications
    }
}