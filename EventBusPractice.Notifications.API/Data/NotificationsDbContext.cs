using EventBusPractice.Notifications.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventBusPractice.Notifications.API.Data
{
    public class NotificationsDbContext : DbContext
    {
        public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        
        public DbSet<Notification> Notifications { get; set; }
    }
}
