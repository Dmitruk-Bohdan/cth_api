using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CTHelper.Domain.Entities;

namespace CTHelper.Persistence.Configurations
{
    public class TestAttemptConfiguration : IEntityTypeConfiguration<TestAttempt>
    {
        public void Configure(EntityTypeBuilder<TestAttempt> builder)
        {
            builder.ToTable("test_attempt", t =>
            {
                t.HasCheckConstraint(
                    "CK_test_attempt_positive_values",
                    "(duration IS NULL OR duration >= 0) AND (raw_score IS NULL OR raw_score >= 0)"
                );
            });

            builder.HasKey(ta => ta.Id);

            builder.Property(ta => ta.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");

            builder.Property(ta => ta.TestId)
                .HasColumnName("test_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(ta => ta.StudentId)
                .HasColumnName("student_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(ta => ta.Status)
                .HasColumnName("status")
                .HasConversion<short>()
                .IsRequired();

            builder.Property(ta => ta.Duration)
                .HasColumnName("duration");

            builder.Property(ta => ta.RawScore)
                .HasColumnName("raw_score");

            builder.Property(ta => ta.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.HasOne(ta => ta.Test)
                .WithMany()
                .HasForeignKey(ta => ta.TestId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasPrincipalKey(u => u.Id);

            builder.HasOne(ta => ta.Student)
                .WithMany(s => s.TestAttempts)
                .HasForeignKey(ta => ta.StudentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasPrincipalKey(u => u.Id);

            builder.HasIndex(ta => new { ta.TestId, ta.StudentId });

            builder.HasIndex(us => us.StudentId);
        }
    }
}
