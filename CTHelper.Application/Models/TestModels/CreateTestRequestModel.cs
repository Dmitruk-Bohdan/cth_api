using CTHelper.Domain.Common.Enums;
using CTHelper.Domain.Entities;
using System.Reflection;

namespace CTHelper.Application.Models.TestModels
{
    public class CreateTestRequestModel
    {
        public string Title { get; set; } = default!;
        public long SubjectId { get; set; }
        public long AuthorId { get; set; }
        public bool IsTraning { get; set; }
        public bool IsPublished { get; set; }
        public bool IsPublic { get; set; }
        public int? Duration { get; set; }
        public int? AttemptsCount { get; set; }
        public IEnumerable<TestProblemCodeModel> TestProblemList { get; set; } = new List<TestProblemCodeModel>();
    }

    public class TestProblemCodeModel
    {
        public long ProblemId { get; set; }
        public string Code { get; set; } = default!;
    }
}
