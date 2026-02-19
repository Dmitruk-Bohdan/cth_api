using CTHelper.Domain.Common.Enums;

namespace CTHelper.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = default!;

        public string PasswordHash { get; set; } = default!;

        public string Email { get; set; } = default!;

        public Role Role { get; set; }

        public DateTimeOffset? LastLoginAt { get; set; }

        public string? AvatarUrl { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset LastUpdateAt { get; set; }

        public ICollection<Assignment> IssuedAssignments { get; set; } = new List<Assignment>();

        public ICollection<Assignment> RecievedAssignments { get; set; } = new List<Assignment>();

        public ICollection<Test> AuthoredTests { get; set; } = new List<Test>();
        public ICollection<Problem> AuthoredProblems { get; set; } = new List<Problem>();

        public ICollection<TestAttempt> TestAttempts { get; set; } = new List<TestAttempt>();

        public ICollection<UserSession> Sessions { get; set; } = new List<UserSession>();

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        public ICollection<TeacherStudent> Teachers { get; set; } = new List<TeacherStudent>();

        public ICollection<TeacherStudent> Students { get; set; } = new List<TeacherStudent>();

        public ICollection<Group> Groups { get; set; } = new List<Group>();

        public ICollection<InvitationCode> InvitationCodes { get; set; } = new List<InvitationCode>();

        public ICollection<Test> FavoriteTests { get; set; } = new List<Test>();

        public ICollection<Problem> FavoriteProblems { get; set; } = new List<Problem>();

    }
}
