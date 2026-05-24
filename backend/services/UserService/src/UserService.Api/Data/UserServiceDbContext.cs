using Microsoft.EntityFrameworkCore;


namespace UserService.Api.Data;

public class UserServiceDbContext : DbContext
{
    public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}