using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CTHelper.Domain.Entities;

namespace CTHelper.Persistence.Configurations
{
    public class ProblemConfiguration : IEntityTypeConfiguration<Problem>
    {
        public void Configure(EntityTypeBuilder<Problem> builder)
        {
            builder.ToTable("problem");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");

            builder.Property(pv => pv.AuthorId)
                .HasColumnName("author_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(p => p.TopicId)
                .HasColumnName("topic_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(p => p.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false);

            builder.HasOne(p => p.Topic)
                .WithMany(t => t.Problems)
                .HasForeignKey(p => p.TopicId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasPrincipalKey(u => u.Id);


            builder.HasOne(pv => pv.Author)
                .WithMany(a => a.AuthoredProblems)
                .HasForeignKey(pv => pv.AuthorId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasPrincipalKey(u => u.Id);

            builder.HasIndex(us => us.TopicId);
        }
    }
}
