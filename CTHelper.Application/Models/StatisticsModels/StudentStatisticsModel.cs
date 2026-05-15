namespace CTHelper.Application.Models.Statistics
{
    public class StudentStatisticsModel
    {
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        public int CommonRate { get; set; }
        public int MedianRate { get; set; }
        public int TotalAnswers { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalAttempts { get; set; }
        public List<TopicStatisticsModel> StatisticsByTopicList { get; set; } = new();
        public List<TopicModel> PendingTopicList { get; set; } = new();
        public List<TopicModel> TopicToReviewList { get; set; } = new();
    }
}
