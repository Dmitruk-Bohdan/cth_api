using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CTHelper.Persistence.Audit;

namespace CTHelper.Persistence.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("audit_log");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");

            builder.Property(a => a.EntityType)
                .HasColumnName("entity_type")
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(a => a.EntityId)
                .HasColumnName("entity_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(a => a.Action)
                .HasColumnName("action")
                .HasConversion<short>()
                .IsRequired();

            builder.Property(a => a.Changes)
                .HasColumnName("changes")
                .HasColumnType("jsonb")
                .HasDefaultValue("{}");

            builder.Property(a => a.Timestamp)
                .HasColumnName("timestamp")
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.HasIndex(a => new { a.EntityType, a.EntityId });
        }
    }
}
