using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CTHelper.Domain.Entities;

namespace CTHelper.Persistence.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
      
            builder.ToTable("image");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");

            builder.Property(i => i.ObjectKey)
                .HasColumnName("object_key")
                .HasColumnType("text")
                .IsRequired();

            builder.Property(i => i.OwnerId)
                .HasColumnName("owner_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(i => i.ContentType)
                .HasColumnName("content_type")
                .HasColumnType("text")
                .IsRequired();

            builder.Property(i => i.Size)
                .HasColumnName("size")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(i => i.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.HasIndex(i => i.ObjectKey)
                .IsUnique();

            builder.HasIndex(i => i.OwnerId);
        }
    }
}
