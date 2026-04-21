namespace CTHelper.Application.Models.TestModels
{
    public class UpdateTestRequestModel
    {
        public long UserId { get; set; }
        public long TestId { get; set; } = default!;
        public string Title { get; set; } = default!;
        public bool IsTraning { get; set; }
        public bool IsPublished { get; set; }
        public bool IsPublic { get; set; }
        public int? Duration { get; set; }
        public int? AttemptsCount { get; set; }
        public IEnumerable<TestProblemCodeModel> TestProblemIdList { get; set; } = new List<TestProblemCodeModel>();
    }
}
