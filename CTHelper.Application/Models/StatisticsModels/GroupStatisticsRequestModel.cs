namespace CTHelper.Application.Models.Statistics
{
    public class GroupStatisticsBySubjectRequestModel
    {
        public long GroupId { get; set; }
        public long UserId { get; set; }
        public long SubjectId { get; set; }
        public DateTimeOffset? DateFrom { get; set; }
        public DateTimeOffset? DateTo { get; set; }
    }
}
