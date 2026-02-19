namespace CTHelper.Domain.Entities
{
    public class Problem : BaseEntity
    {
        public long TopicId { get; set; }
        public long AuthorId { get; set; }

        public bool IsDeleted { get; set; }

        public Topic Topic { get; set; } = default!;
        public User Author { get; set; } = default!;


        public ICollection<ProblemVersion> Versions { get; set; } = new List<ProblemVersion>();

        public ICollection<TestProblem> TestProblems { get; set; } = new List<TestProblem>();

    }
}


