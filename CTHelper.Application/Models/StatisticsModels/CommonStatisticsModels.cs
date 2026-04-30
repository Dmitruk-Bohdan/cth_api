using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.Statistics
{
    public class TopicModel
    {
        public long TopicId { get; set; }
        public string TopicName { get; set; } = default!;
    }

    public class TopicStatisticsModel : TopicModel
    {
        public int AverageSuccessRate { get; set; } = default!;

        public List<SuccessByDifficultModel> SuccessRateByDifficultList = new();
    }

    public class SuccessByDifficultModel
    {
        public ProblemDifficultEnum Difficult { get; set; }
        public int SuccessRate { get; set; }
    }
}
