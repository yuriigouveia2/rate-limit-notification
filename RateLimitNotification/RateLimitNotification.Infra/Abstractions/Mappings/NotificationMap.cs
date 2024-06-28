using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RateLimitNotification.Domain.Notification.Entities;

namespace RateLimitNotification.Infra.Abstractions.Mappings
{
    public class NotificationMap : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("notification");

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasColumnName("user_id");

            builder.Property(x => x.NotificationType)
                .IsRequired()
                .HasColumnName("notification_type");

            builder.Property(x => x.Message)
                .IsRequired()
                .HasColumnName("message");

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
