using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using СTHelper.Domain.Entities;

namespace СTHelper.Persistence.Configurations
{
    public class UserAnswerConfiguration : IEntityTypeConfiguration<UserAnswer>
    {
        public void Configure(EntityTypeBuilder<UserAnswer> builder)
        {
            builder.ToTable("user_answer");

            builder.HasKey(ua => ua.Id);

            builder.Property(ua => ua.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");

            builder.Property(ua => ua.TestAttemptId)
                .HasColumnName("test_attempt_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(ua => ua.ProblemVersionId)
                .HasColumnName("problem_version_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(ua => ua.Answer)
                .HasColumnName("answer")
                .HasMaxLength(32);

            builder.Property(ua => ua.IsCorrect)
                .HasColumnName("is_correct");

            builder.Property(ua => ua.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.HasOne(ua => ua.TestAttempt)
                .WithMany()
                .HasForeignKey(ua => ua.TestAttemptId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ua => ua.ProblemVersion)
                .WithMany()
                .HasForeignKey(ua => ua.ProblemVersionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(us => us.TestAttemptId);
        }
    }
}
