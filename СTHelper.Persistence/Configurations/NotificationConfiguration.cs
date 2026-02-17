using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using СTHelper.Domain.Entities;

namespace СTHelper.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("notification");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");

            builder.Property(n => n.RecipientId)
                .HasColumnName("recipient_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(n => n.PriorityLevel)
                .HasColumnName("priority_level")
                .IsRequired();

            builder.Property(n => n.Payload)
                .HasColumnName("payload")
                .HasColumnType("text");

            builder.Property(n => n.IsSeen)
                .HasColumnName("is_seen")
                .HasDefaultValue(false);

            builder.Property(n => n.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.HasOne(n => n.Recipient)
                .WithMany()
                .HasForeignKey(n => n.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
