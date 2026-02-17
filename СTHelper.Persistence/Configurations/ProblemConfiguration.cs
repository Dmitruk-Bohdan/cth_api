using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using СTHelper.Domain.Entities;

namespace СTHelper.Persistence.Configurations
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

            builder.Property(p => p.TopicId)
                .HasColumnName("topic_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(p => p.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false);

            builder.HasOne(p => p.Topic)
                .WithMany()
                .HasForeignKey(p => p.TopicId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
