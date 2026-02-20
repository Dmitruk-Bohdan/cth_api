using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using CTHelper.Domain.Entities;

namespace CTHelper.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");

            builder.Property(u => u.Username)
                .HasColumnName("username")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.PasswordHash) //SHA512
                .HasColumnName("password_hash")
                .HasColumnType("char(128)")
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasMaxLength(254)
                .IsRequired();

            builder.Property(u => u.Role)
                .HasColumnName("role")
                .HasConversion<short>()
                .IsRequired();

            builder.Property(u => u.LastLoginAt)
                .HasColumnName("last_login_at")
                .HasColumnType("timestamptz");

            builder.Property(u => u.AvatarUrl)
                .HasColumnName("avatar_url")
                .HasMaxLength(2048);

            builder.Property(u => u.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false);

            builder.Property(u => u.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(u => u.LastUpdateAt)
                .HasColumnName("last_update_at")
                .HasColumnType("timestamptz")
                .IsRequired();

            builder
                .HasMany(u => u.Groups)
                .WithMany(g => g.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "student_group_student",
                     sgs => sgs.HasOne<Group>()
                        .WithMany()
                        .HasForeignKey("group_id"),
                     sgs => sgs.HasOne<User>()
                        .WithMany()
                        .HasForeignKey("student_id"))
                .HasIndex("group_id");

            builder
                .HasMany(u => u.FavoriteTests)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "favorite_test",
                    ft => ft.HasOne<Test>()
                        .WithMany()
                        .HasForeignKey("test_id"),
                    ft => ft.HasOne<User>()
                        .WithMany()
                        .HasForeignKey("student_id"))
                .HasIndex("student_id");

            builder
                .HasMany(u => u.FavoriteProblems)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "favorite_problem",
                    fp => fp.HasOne<Problem>()
                        .WithMany()
                        .HasForeignKey("problem_id"),
                    fp => fp.HasOne<User>()
                        .WithMany()
                        .HasForeignKey("student_id"))
                    .HasIndex("student_id");

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.Username).IsUnique();
        }
    }
}
