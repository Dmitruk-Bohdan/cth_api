using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using СTHelper.Domain.Entities;

namespace СTHelper.Persistence.Configurations
{
    public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
    {
        public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
        {
            builder.ToTable("password_reset_token");

            builder.HasKey(prt => prt.Id);

            builder.Property(prt => prt.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");

            builder.Property(prt => prt.UserId)
                .HasColumnName("user_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(prt => prt.TokenHash)
                .HasColumnName("token_hash")
                .IsRequired();

            builder.Property(prt => prt.ExpiresAt)
                .HasColumnName("expires_at")
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.Property(prt => prt.UsedAt)
                .HasColumnName("used_at")
                .HasColumnType("timestamptz");
        }
    }
}
