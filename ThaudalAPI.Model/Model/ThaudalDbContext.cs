using Microsoft.EntityFrameworkCore;
using ThaudalAPI.Model.Model.Users;

namespace ThaudalAPI.Model.Model;

public class ThaudalDbContext : DbContext
{
    public ThaudalDbContext(DbContextOptions options) : base(options)
    {
        
    }
    public DbSet<User> Users { get; set; }
}