using EventBusPractice.Users.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventBusPractice.Users.API.Data
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
