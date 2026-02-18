using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using СTHelper.Domain.Entities;

namespace СTHelper.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_token");

            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");

            builder.Property(rt => rt.SessionId)
                .HasColumnName("session_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(rt => rt.TokenHash) //SHA512
                .HasColumnType("char(128)")
                .HasColumnName("token_hash")
                .IsRequired();

            builder.Property(rt => rt.ExpiresAt)
                .HasColumnName("expires_at")
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.Property(rt => rt.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(rt => rt.RevokedAt)
                .HasColumnName("revoked_at")
                .HasColumnType("timestamptz");

            builder.HasIndex(rt => rt.TokenHash)
                .IsUnique();

            builder.HasIndex(rt => rt.SessionId)
                .IsUnique()
                .HasFilter("revoked_at IS NULL");

            builder.HasIndex(rt => rt.ExpiresAt);

        }
    }
}
