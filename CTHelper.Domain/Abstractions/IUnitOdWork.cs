using CTHelper.Domain.Entities;

namespace CTHelper.Domain.Abstractions
{
    public interface IUnitOdWork : IAsyncDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;

        IRepository<Assignment> Assignments { get; }
        IRepository<Group> Groups { get; }
        IRepository<InvitationCode> InvitationCodes { get; }
        IRepository<Notification> Notifications { get; }
        IRepository<Problem> Problems { get; }
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
