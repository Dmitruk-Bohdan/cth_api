using СTHelper.Domain.Common.Enums;

namespace СTHelper.Domain.Entities
{
    public class Test : BaseEntity
    {
        public string Title { get; set; } = default!;

        public Subject Subject { get; set; }

        public long AuthorId { get; set; }

        public TestType Type { get; set; }

        public bool IsTraning { get; set; }

        public bool IsPublished { get; set; }

        public bool IsPublic { get; set; }

        public bool IsDeleted { get; set; }
        public int? Duration { get; set; }
        public short? AttemptsCount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset LastUpdateAt { get; set; }

        public User Author { get; set; } = default!;

        public ICollection<TestProblem> TestProblems { get; set; } = new List<TestProblem>();

        public ICollection<TestAttempt> Attempts { get; set; } = new List<TestAttempt>();

        public ICollection<User> FavoriteByStudents { get; set; } = new List<User>();

        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}
