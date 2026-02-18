using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using СTHelper.Domain.Entities;

namespace СTHelper.Persistence.Configurations
{
    public class ConnectionRequestConfiguration : IEntityTypeConfiguration<ConnectionRequest>
    {
        public void Configure(EntityTypeBuilder<ConnectionRequest> builder)
        {
            builder.ToTable("connection_request");

            builder.HasKey(cr => cr.Id);

            builder.Property(cr => cr.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");

            builder.Property(cr => cr.CodeId)
                .HasColumnName("code_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(cr => cr.StudentId)
                .HasColumnName("student_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(cr => cr.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(cr => cr.LastUpdateAt)
                .HasColumnName("last_update_at")
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.HasOne(cr => cr.Code)
                .WithMany()
                .HasForeignKey(cr => cr.CodeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cr => cr.Student)
                .WithMany()
                .HasForeignKey(cr => cr.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(us => us.CodeId);
        }
    }
}
