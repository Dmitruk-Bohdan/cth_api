using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using СTHelper.Domain.Entities;

namespace СTHelper.Persistence.Configurations
{
    public class TestAttemptConfiguration : IEntityTypeConfiguration<TestAttempt>
    {
        public void Configure(EntityTypeBuilder<TestAttempt> builder)
        {
            builder.ToTable("test_attempt");

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
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ta => ta.Student)
                .WithMany()
                .HasForeignKey(ta => ta.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
