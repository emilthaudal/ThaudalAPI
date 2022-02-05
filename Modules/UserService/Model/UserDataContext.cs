using Microsoft.EntityFrameworkCore;
using UserService.Model.Users;

namespace UserService.Model;

public class UserDataContext : DbContext
{
    public UserDataContext(DbContextOptions options) : base(options)
    {
        
    }
    public DbSet<User> Users { get; set; }
}