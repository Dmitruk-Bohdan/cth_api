using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.Statistics
{
    public class TopicModel
    {
        public long TopicId { get; set; }
        public string TopicName { get; set; } = default!;
    }

    public class TopicStatisticsModel
    {
        public long TopicId { get; set; }
        public string TopicName { get; set; } = default!;
        public int AverageSuccessRate { get; set; }
        public int MedianSuccessRate { get; set; }
        public List<SuccessByDifficultModel> SuccessRateByDifficultList { get; set; } = new();
    }

    public class SuccessByDifficultModel
    {
        public int Difficult { get; set; }
        public int SuccessRate { get; set; }
        public int MedianSuccessRate { get; set; }
    }
}
