using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CTHelper.Domain.Entities;
using CTHelper.Persistence.Audit;
using CTHelper.Persistence.Entities;

namespace CTHelper.Persistence.Context
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

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);
        }

    }
}
