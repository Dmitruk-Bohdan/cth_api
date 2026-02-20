using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;
using CTHelper.Persistence.Context;

namespace CTHelper.Persistence.Repositories
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Lazy<IRepository<Assignment>> _assignmentRepository;
        private readonly Lazy<IRepository<ConnectionRequest>> _connectionRequestRepository;
        private readonly Lazy<IRepository<EmailVerificationToken>> _emailVerificationTokenRepository;
        private readonly Lazy<IRepository<Group>> _groupRepository;
        private readonly Lazy<IRepository<InvitationCode>> _invitationCodeRepository;
        private readonly Lazy<IRepository<Notification>> _notificationRepository;
        private readonly Lazy<IRepository<PasswordResetToken>> _passwordResetTokenRepository;
        private readonly Lazy<IRepository<Problem>> _problemRepository;
        private readonly Lazy<IRepository<RefreshToken>> _refreshTokenRepository;
        private readonly Lazy<IRepository<ProblemVersion>> _problemVersionRepository;
        private readonly Lazy<IRepository<TeacherStudent>> _teacherStudentRepository;
        private readonly Lazy<IRepository<Test>> _testRepository;
        private readonly Lazy<IRepository<TestAttempt>> _testAttemptRepository;
        private readonly Lazy<IRepository<TestProblem>> _testProblemRepository;
        private readonly Lazy<IRepository<Topic>> _topicRepository;
        private readonly Lazy<IRepository<User>> _userRepository;
        private readonly Lazy<IRepository<UserAnswer>> _userAnswerRepository;
        private readonly Lazy<IRepository<UserSession>> _userSessionRepository;

        public EfUnitOfWork(AppDbContext context)
        {
            _context = context;

            _assignmentRepository = new Lazy<IRepository<Assignment>>(() =>
                new EfRepository<Assignment>(context));

            _connectionRequestRepository = new Lazy<IRepository<ConnectionRequest>>(() =>
                new EfRepository<ConnectionRequest>(context));

            _emailVerificationTokenRepository = new Lazy<IRepository<EmailVerificationToken>>(() =>
                new EfRepository<EmailVerificationToken>(context));

            _groupRepository = new Lazy<IRepository<Group>>(() =>
                new EfRepository<Group>(context));

            _invitationCodeRepository = new Lazy<IRepository<InvitationCode>>(() =>
                new EfRepository<InvitationCode>(context));

            _notificationRepository = new Lazy<IRepository<Notification>>(() =>
                new EfRepository<Notification>(context));

            _passwordResetTokenRepository = new Lazy<IRepository<PasswordResetToken>>(() =>
                new EfRepository<PasswordResetToken>(context));

            _problemRepository = new Lazy<IRepository<Problem>>(() =>
                new EfRepository<Problem>(context));

            _refreshTokenRepository = new Lazy<IRepository<RefreshToken>>(() =>
                new EfRepository<RefreshToken>(context));

            _problemVersionRepository = new Lazy<IRepository<ProblemVersion>>(() =>
                new EfRepository<ProblemVersion>(context));

            _teacherStudentRepository = new Lazy<IRepository<TeacherStudent>>(() =>
                new EfRepository<TeacherStudent>(context));

            _testRepository = new Lazy<IRepository<Test>>(() =>
                new EfRepository<Test>(context));

            _testAttemptRepository = new Lazy<IRepository<TestAttempt>>(() =>
                new EfRepository<TestAttempt>(context));

            _testProblemRepository = new Lazy<IRepository<TestProblem>>(() =>
                new EfRepository<TestProblem>(context));

            _topicRepository = new Lazy<IRepository<Topic>>(() =>
                new EfRepository<Topic>(context));

            _userRepository = new Lazy<IRepository<User>>(() =>
                new EfRepository<User>(context));

            _userAnswerRepository = new Lazy<IRepository<UserAnswer>>(() =>
                new EfRepository<UserAnswer>(context));

            _userSessionRepository = new Lazy<IRepository<UserSession>>(() =>
                new EfRepository<UserSession>(context));
        }

        public IRepository<Assignment> Assignments => _assignmentRepository.Value;
        public IRepository<ConnectionRequest> ConnectionRequests => _connectionRequestRepository.Value;
        public IRepository<EmailVerificationToken> EmailVerificationTokens => _emailVerificationTokenRepository.Value;
        public IRepository<Group> Groups => _groupRepository.Value;
        public IRepository<InvitationCode> InvitationCodes => _invitationCodeRepository.Value;
        public IRepository<Notification> Notifications => _notificationRepository.Value;
        public IRepository<PasswordResetToken> PasswordResetTokens => _passwordResetTokenRepository.Value;
        public IRepository<Problem> Problems => _problemRepository.Value;
        public IRepository<RefreshToken> RefreshTokens => _refreshTokenRepository.Value;
        public IRepository<ProblemVersion> ProblemVersions => _problemVersionRepository.Value;
        public IRepository<TeacherStudent> TeacherStudents => _teacherStudentRepository.Value;
        public IRepository<Test> Tests => _testRepository.Value;
        public IRepository<TestAttempt> TestAttempts => _testAttemptRepository.Value;
        public IRepository<TestProblem> TestProblems => _testProblemRepository.Value;
        public IRepository<Topic> Topics => _topicRepository.Value;
        public IRepository<User> Users => _userRepository.Value;
        public IRepository<UserAnswer> UserAnswers => _userAnswerRepository.Value;
        public IRepository<UserSession> UserSessions => _userSessionRepository.Value;

        public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
            => new EfRepository<TEntity>(_context);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _context.SaveChangesAsync(cancellationToken);

        public ValueTask DisposeAsync()
            => _context.DisposeAsync();
    }
}
