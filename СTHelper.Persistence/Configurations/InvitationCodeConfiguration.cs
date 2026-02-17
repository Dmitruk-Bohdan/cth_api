using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using СTHelper.Domain.Entities;

namespace СTHelper.Persistence.Configurations
{
    public class InvitationCodeConfiguration : IEntityTypeConfiguration<InvitationCode>
    {
        public void Configure(EntityTypeBuilder<InvitationCode> builder)
        {
            builder.ToTable("invitation_code");

            builder.HasKey(ic => ic.Id);

            builder.Property(ic => ic.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");

            builder.Property(ic => ic.TeacherId)
                .HasColumnName("teacher_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(ic => ic.Code)
                .HasColumnName("code")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(ic => ic.UsesLeft)
                .HasColumnName("uses_left")
                .IsRequired();

            builder.Property(ic => ic.IsRevoked)
                .HasColumnName("is_revoked")
                .HasDefaultValue(false);

            builder.Property(ic => ic.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(ic => ic.LastUpdateAt)
                .HasColumnName("last_update_at")
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.HasOne(ic => ic.Teacher)
                .WithMany()
                .HasForeignKey(ic => ic.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
