using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using СTHelper.Domain.Entities;

namespace СTHelper.Persistence.Configurations
{
    public class TeacherStudentConfiguration : IEntityTypeConfiguration<TeacherStudent>
    {
        public void Configure(EntityTypeBuilder<TeacherStudent> builder)
        {
            builder.ToTable("teacher_student");

            builder.HasKey(ts => ts.Id);

            builder.Property(ts => ts.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");

            builder.Property(ts => ts.TeacherId)
                .HasColumnName("teacher_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(ts => ts.StudentId)
                .HasColumnName("student_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(ts => ts.Status)
                .HasColumnName("status")
                .IsRequired();

            builder.Property(ts => ts.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false);

            builder.Property(ts => ts.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(ts => ts.LastUpdateAt)
                .HasColumnName("last_update_at")
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.HasOne(ts => ts.Teacher)
                .WithMany()
                .HasForeignKey(ts => ts.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ts => ts.Student)
                .WithMany()
                .HasForeignKey(ts => ts.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
