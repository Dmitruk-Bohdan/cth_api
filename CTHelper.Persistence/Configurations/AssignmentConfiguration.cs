using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CTHelper.Domain.Entities;

namespace CTHelper.Persistence.Configurations
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
                t.HasCheckConstraint(
                    "CK_assignment_positive_values",
                    "attempts_left >= 0"
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
                .WithMany(s => s.RecievedAssignments)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasPrincipalKey(u => u.Id);

            builder.HasOne(a => a.Teacher)
                .WithMany(t => t.IssuedAssignments)
                .HasForeignKey(a => a.TeacherId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasPrincipalKey(u => u.Id);

            builder.HasOne(a => a.Group)
                .WithMany(g => g.ReceivedAssignments)
                .HasForeignKey(a => a.GroupId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasPrincipalKey(u => u.Id);

            builder.HasOne(a => a.Test)
                .WithMany()
                .HasForeignKey(a => a.TestId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasPrincipalKey(u => u.Id);

            builder.HasIndex(a => a.StudentId); 
            builder.HasIndex(a => a.GroupId);   
            builder.HasIndex(a => a.TeacherId); 
        }
    }
}
