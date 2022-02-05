using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserService.Model.Users;

namespace UserService.Model;

public class UserDataContext : DbContext
{
    public DbSet<User> Users { get; set; }

    private readonly IConfiguration _configuration;

    public UserDataContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // in memory database used for simplicity, change to a real db for production applications
    }
}