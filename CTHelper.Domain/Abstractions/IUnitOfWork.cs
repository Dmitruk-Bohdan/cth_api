using CTHelper.Domain.Entities;

namespace CTHelper.Domain.Abstractions
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        IRepository<Assignment> Assignments { get; }
        IRepository<ConnectionRequest> ConnectionRequests { get; }
        IRepository<EmailVerificationToken> EmailVerificationTokens { get; }
        IRepository<Group> Groups { get; }
        IRepository<InvitationCode> InvitationCodes { get; }
        IRepository<Notification> Notifications { get; }
        IRepository<PasswordResetToken> PasswordResetTokens { get; }
        IRepository<Problem> Problems { get; }
        IRepository<RefreshToken> RefreshTokens { get; }
        IRepository<ProblemVersion> ProblemVersions { get; }
        IRepository<TeacherStudent> TeacherStudents { get; }
        IRepository<Test> Tests { get; }
        IRepository<TestAttempt> TestAttempts { get; }
        IRepository<TestProblem> TestProblems { get; }
        IRepository<Topic> Topics { get; }
        IRepository<User> Users { get; }
        IRepository<UserAnswer> UserAnswers { get; }
        IRepository<UserSession> UserSessions { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
