using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using СTHelper.Domain.Entities;

namespace СTHelper.Persistence.Configurations
{
    public class TestProblemConfiguration : IEntityTypeConfiguration<TestProblem>
    {
        public void Configure(EntityTypeBuilder<TestProblem> builder)
        {
            builder.ToTable("test_problem");

            builder.HasKey(tp => tp.Id);

            builder.Property(tp => tp.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");

            builder.Property(tp => tp.TestId)
                .HasColumnName("test_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(tp => tp.ProblemId)
                .HasColumnName("problem_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(tp => tp.Code)
                .HasColumnName("code")
                .HasMaxLength(3);

            builder.Property(tp => tp.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false);

            builder.Property(tp => tp.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.HasOne(tp => tp.Test)
                .WithMany()
                .HasForeignKey(tp => tp.TestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(tp => tp.Problem)
                .WithMany()
                .HasForeignKey(tp => tp.ProblemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
