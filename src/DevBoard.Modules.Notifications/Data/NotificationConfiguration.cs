using DevBoard.Modules.Notifications.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevBoard.Modules.Notifications.Data;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications", "notifications");

        builder.HasKey(notification => notification.Id);

        builder.Property(notification => notification.Type)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(notification => notification.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(notification => notification.Message)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(notification => notification.IsRead)
            .IsRequired();

        builder.Property(notification => notification.CreatedAt)
            .IsRequired();

        builder.Property(notification => notification.ReadAt);

        builder.Property(notification => notification.RelatedProjectId);
        builder.Property(notification => notification.RelatedTaskId);

        builder.HasIndex(notification => notification.IsRead);
        builder.HasIndex(notification => notification.CreatedAt);
        builder.HasIndex(notification => notification.RelatedProjectId);
        builder.HasIndex(notification => notification.RelatedTaskId);
    }
}
