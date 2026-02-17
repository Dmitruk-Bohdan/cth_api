using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using СTHelper.Domain.Entities;
using СTHelper.Persistence.Audit;
using СTHelper.Persistence.Entities;

namespace СTHelper.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Domain Entities
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<ConnectionRequest> ConnectionRequests { get; set; }
        public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<InvitationCode> InvitationCodes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<ProblemVersion> ProblemVersions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<TeacherStudent> TeacherStudents { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestAttempt> TestAttempts { get; set; }
        public DbSet<TestProblem> TestProblems { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }

        // Persistence Audit
        public DbSet<AuditLog> AuditLogs { get; set; }

        // Persistence Events
        public DbSet<UserEvent> UserEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationships using Fluent API
            // Student-Group (student_group_student)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Groups)
                .WithMany(g => g.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "student_group_student",
                    j => j.HasOne<Group>().WithMany().HasForeignKey("group_id"),
                    j => j.HasOne<User>().WithMany().HasForeignKey("student_id"));

            // Student-Test favorites (favorite_tests)
            modelBuilder.Entity<User>()
                .HasMany(u => u.FavoriteTests)
                .WithMany(t => t.FavoriteByStudents)
                .UsingEntity<Dictionary<string, object>>(
                    "favorite_tests",
                    j => j.HasOne<Test>().WithMany().HasForeignKey("test_id"),
                    j => j.HasOne<User>().WithMany().HasForeignKey("student_id"));

            // Student-Problem favorites (favorite_problem)
            modelBuilder.Entity<User>()
                .HasMany(u => u.FavoriteProblems)
                .WithMany(p => p.FavoriteByStudents)
                .UsingEntity<Dictionary<string, object>>(
                    "favorite_problem",
                    j => j.HasOne<Problem>().WithMany().HasForeignKey("problem_id"),
                    j => j.HasOne<User>().WithMany().HasForeignKey("student_id"));

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);
        }

    }
}
