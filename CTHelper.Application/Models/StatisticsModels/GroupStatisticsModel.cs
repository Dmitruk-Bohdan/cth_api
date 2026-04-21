namespace CTHelper.Application.Models.Statistics
{
    public class GroupStatisticsModel
    {
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public List<GroupMemberStatisticItem> Members { get; set; } = new();
        public List<TopicStatisticsModel> StatisticsByTopicList { get; set; } = new();
        public List<TopicModel> PendingTopicList { get; set; } = new(); //не изучались вообще
        public List<TopicModel> TopicToReviewList { get; set; } = new(); // изучались, но повторялись меньше остальных
    }

    public class GroupMemberStatisticItem
    {
        public long StudentId { get; set; }
        public string StudentName { get; set; } = default!;
        public int StudentRate {  get; set; }
        public int StudentGroupRating { get; set; }
    }
}
