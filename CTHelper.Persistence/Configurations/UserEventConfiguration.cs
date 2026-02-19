using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CTHelper.Persistence.Entities;

namespace CTHelper.Persistence.Configurations
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
                .HasConversion<short>()
                .HasColumnName("event_type")
                .IsRequired();

            builder.Property(ue => ue.TargetId)
                .HasColumnName("target_id")
                .HasColumnType("bigint");

            builder.Property(ue => ue.IpAddress)
                .HasColumnType("inet")
                .HasConversion(new StringToInetConverter())
                .HasColumnName("ip_address");

            builder.Property(ue => ue.DeviceInfo)
                .HasColumnType("jsonb")
                .HasColumnName("device_info");

            builder.Property(ue => ue.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.HasOne(ue => ue.User)
                .WithMany()
                .HasForeignKey(ue => ue.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasPrincipalKey(u => u.Id);

            builder.HasIndex(us => us.UserId);
        }
    }
}
