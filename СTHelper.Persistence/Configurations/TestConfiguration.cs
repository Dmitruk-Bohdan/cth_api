using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using СTHelper.Domain.Entities;

namespace СTHelper.Persistence.Configurations
{
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.ToTable("test", t =>
            {
                t.HasCheckConstraint(
                    "CK_test_positive_values",
                    "attempts_count >= 0 AND (duration IS NULL OR duration >= 0)"
                );
            });

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");

            builder.Property(t => t.Title)
                .HasColumnName("title")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(t => t.Subject)
                .HasConversion<short>()
                .HasColumnName("subject")
                .IsRequired();

            builder.Property(t => t.AuthorId)
                .HasColumnName("author_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(t => t.Type)
                .HasConversion<short>()
                .HasColumnName("type")
                .IsRequired();

            builder.Property(t => t.IsTraning)
                .HasColumnName("is_traning")
                .HasDefaultValue(false);

            builder.Property(t => t.IsPublished)
                .HasColumnName("is_published")
                .HasDefaultValue(false);

            builder.Property(t => t.IsPublic)
                .HasColumnName("is_public")
                .HasDefaultValue(false);

            builder.Property(t => t.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false);

            builder.Property(t => t.AttemptsCount)
                .HasColumnName("attempts_count");

            builder.Property(t => t.Duration)
                .HasColumnName("duration");

            builder.Property(t => t.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(t => t.LastUpdateAt)
                .HasColumnName("last_update_at")
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.HasOne(t => t.Author)
                .WithMany()
                .HasForeignKey(t => t.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(t => t.Title);

            builder.HasIndex(t => t.Subject);

            builder.HasQueryFilter(t => !t.IsDeleted);

            builder.HasIndex(us => us.AuthorId);
        }
    }
}
