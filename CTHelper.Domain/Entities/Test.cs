using CTHelper.Domain.Common.Enums;

namespace CTHelper.Domain.Entities
{
    public class Test : BaseEntity
    {
        public string Title { get; set; } = default!;
        public long SubjectId { get; set; }
        public long AuthorId { get; set; }
        public TestTypeEnum Type { get; set; }
        public bool IsTraning { get; set; }
        public bool IsPublished { get; set; }
        public bool IsPublic { get; set; }
        public bool IsDeleted { get; set; }
        public int Duration { get; set; }
        public int AttemptsCount { get; set; }
        public Subject Subject { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdateAt { get; set; }
        public User Author { get; set; } = default!;
        public ICollection<TestProblem> TestProblems { get; set; } = new List<TestProblem>();
        public ICollection<GroupAssignment> GroupAssignments { get; set; } = new List<GroupAssignment>();
        public ICollection<StudentAssignment> StudentAssignments { get; set; } = new List<StudentAssignment>();
    }
}
