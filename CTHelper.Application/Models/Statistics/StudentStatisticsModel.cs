namespace CTHelper.Application.Models.Statistics
{
    public class StudentStatisticsModel
    {
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public int CommonRate { get; set; }
        public int TotalAttempts { get; set; }               
        public int CorrectAttempts { get; set; }
        public List<TopicStatisticsModel> StatisticsByTopicList { get; set; } = new();
        public List<TopicModel> PendingTopicList { get; set; } = new(); //не изучались вообще
        public List<TopicModel> TopicToReviewList { get; set; } = new(); // изучались, но повторялись меньше остальных
    }
}
