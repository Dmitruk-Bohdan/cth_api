using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using СTHelper.Domain.Entities;

namespace СTHelper.Persistence.Configurations
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.ToTable("assignment", t =>
            {
                t.HasCheckConstraint(
                    "CK_assignment_target",
                    "(student_id IS NULL) <> (group_id IS NULL)"
                );
            });


            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");

            builder.Property(a => a.StudentId)
                .HasColumnName("student_id")
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

            builder.Property(a => a.AttemptsLeft)
                .HasColumnName("attempts_left")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(a => a.LastUpdateAt)
                .HasColumnName("last_update_at")
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.HasOne(a => a.Student)
                .WithMany()
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.Teacher)
                .WithMany()
                .HasForeignKey(a => a.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Group)
                .WithMany()
                .HasForeignKey(a => a.GroupId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.Test)
                .WithMany()
                .HasForeignKey(a => a.TestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
