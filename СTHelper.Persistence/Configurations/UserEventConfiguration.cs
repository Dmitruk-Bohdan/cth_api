using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using СTHelper.Persistence.Entities;

namespace СTHelper.Persistence.Configurations
{
    public class UserEventConfiguration : IEntityTypeConfiguration<UserEvent>
    {
        public void Configure(EntityTypeBuilder<UserEvent> builder)
        {
            builder.ToTable("user_event");

            builder.HasKey(ue => ue.Id);

            builder.Property(ue => ue.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");

            builder.Property(ue => ue.UserId)
                .HasColumnName("user_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(ue => ue.EventType)
                .HasColumnName("event_type")
                .IsRequired();

            builder.Property(ue => ue.TargetId)
                .HasColumnName("target_id")
                .HasColumnType("bigint");

            builder.Property(ue => ue.IpAddress)
                .HasColumnName("ip_address");

            builder.Property(ue => ue.DeviceInfo)
                .HasColumnName("device_info");

            builder.Property(ue => ue.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.HasOne(ue => ue.User)
                .WithMany()
                .HasForeignKey(ue => ue.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
