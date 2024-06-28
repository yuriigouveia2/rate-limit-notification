using Microsoft.EntityFrameworkCore;
using RateLimitNotification.Domain.Notification.Entities;

namespace RateLimitNotification.Infra.Abstractions.Context
{
    public class RateLimitNotificationContext : DbContext
    {
        public RateLimitNotificationContext(DbContextOptions<RateLimitNotificationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RateLimitNotificationContext).Assembly);

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(model => model.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }


        #region Definition of Tables in the context of the project
        
        public DbSet<Notification> Notifications { get; set; }

        #endregion
    }
}
