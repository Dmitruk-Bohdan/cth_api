using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using СTHelper.Domain.Entities;

namespace СTHelper.Persistence.Configurations
{
    public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
    {
        public void Configure(EntityTypeBuilder<UserSession> builder)
        {
            builder.ToTable("user_session");

            builder.HasKey(us => us.Id);

            builder.Property(us => us.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");

            builder.Property(us => us.UserId)
                .HasColumnName("user_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(us => us.Jti)
                .HasColumnName("jti");

            builder.Property(us => us.ClientType)
                .HasColumnName("client_type")
                .IsRequired();

            builder.Property(us => us.IpAddress)
                .HasColumnName("ip_address");

            builder.Property(us => us.DeviceInfo)
                .HasColumnName("device_info")
                .HasColumnType("jsonb");

            builder.Property(us => us.LastActivityAt)
                .HasColumnName("last_activity_at")
                .HasColumnType("timestamptz");

            builder.Property(us => us.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(us => us.LastUpdateAt)
                .HasColumnName("last_update_at")
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.Property(us => us.RevokedAt)
                .HasColumnName("revoked_at")
                .HasColumnType("timestamptz");

            builder.HasOne(us => us.User)
                .WithMany(u => u.Sessions)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
