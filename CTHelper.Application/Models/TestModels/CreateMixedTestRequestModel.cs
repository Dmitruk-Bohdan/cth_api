using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.TestModels
{
    public class CreateMixedTestRequestModel
    {
        public long AuthorId { get; set; }
        public long SubjectId { get; set; }
        public ProblemDifficult AverageDifficult { get; set; }
        public IEnumerable<MixedTestTopicModel> TopicItems { get; set; } = new List<MixedTestTopicModel>();
    }
    public class MixedTestTopicModel
    {
        public long TopicId { get; set; }
        public long ProblemCount { get; set; }
    }
}
