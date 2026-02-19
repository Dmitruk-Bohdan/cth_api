using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CTHelper.Domain.Entities;

namespace CTHelper.Persistence.Configurations
{
    public class ProblemVersionConfiguration : IEntityTypeConfiguration<ProblemVersion>
    {
        public void Configure(EntityTypeBuilder<ProblemVersion> builder)
        {
            builder.ToTable("problem_version");

            builder.HasKey(pv => pv.Id);

            builder.Property(pv => pv.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");

            builder.Property(pv => pv.ProblemId)
                .HasColumnName("problem_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(pv => pv.Type)
                .HasColumnName("type")
                .HasConversion<short>()
                .IsRequired();

            builder.Property(pv => pv.Difficulty)
                .HasColumnName("difficulty")
                .HasConversion<short>()
                .IsRequired();

            builder.Property(pv => pv.Statement)
                .HasColumnName("statement")
                .HasColumnType("jsonb")
                .IsRequired();

            builder.Property(pv => pv.CorrectAnswer)
                .HasColumnName("correct_answer")
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(pv => pv.Explanation)
                .HasColumnName("explanation")
                .HasColumnType("jsonb")
                .HasDefaultValue("{}");

            builder.Property(pv => pv.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(false);

            builder.Property(pv => pv.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.HasOne(pv => pv.Problem)
                .WithMany(p => p.Versions)
                .HasForeignKey(pv => pv.ProblemId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasPrincipalKey(u => u.Id);

            builder.HasIndex(us => us.ProblemId);
        }
    }
}
