using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CTHelper.Domain.Entities;

namespace CTHelper.Persistence.Configurations
{
    public class GroupAssignmentConfiguration : IEntityTypeConfiguration<GroupAssignment>
    {
        public void Configure(EntityTypeBuilder<GroupAssignment> builder)
        {
            builder.ToTable("group_assignment", t =>
            {
                t.HasCheckConstraint(
                    "CK_assignment_positive_values",
                    "default_attempts_allowed >= 0"
                );
            });

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");

            builder.Property(a => a.TeacherId)
                .HasColumnName("teacher_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(a => a.GroupId)
                .HasColumnName("group_id")
                .HasColumnType("bigint");

            builder.Property(a => a.TestId)
                .HasColumnName("test_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(a => a.ExpiredAt)
                .HasColumnName("expired_at")
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.Property(t => t.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false);

            builder.Property(a => a.DefaultAttemptsAllowed)
                .HasColumnName("default_attempts_allowed")
                .HasColumnType("smallint");

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(a => a.LastUpdateAt)
                .HasColumnName("last_update_at")
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.HasOne(a => a.Teacher)
                .WithMany(t => t.IssuedGroupAssignments)
                .HasForeignKey(a => a.TeacherId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasPrincipalKey(u => u.Id);

            builder.HasOne(a => a.Group)
                .WithMany(g => g.ReceivedAssignments)
                .HasForeignKey(a => a.GroupId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasPrincipalKey(u => u.Id);

            builder.HasOne(a => a.Test)
                .WithMany(t => t.GroupAssignments)
                .HasForeignKey(a => a.TestId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasPrincipalKey(u => u.Id);

            builder.HasIndex(a => a.GroupId);   
            builder.HasIndex(a => a.TeacherId); 
        }
    }
}
