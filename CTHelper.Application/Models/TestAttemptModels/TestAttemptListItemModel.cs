namespace CTHelper.Application.Models.TestAttemptModels
{
    public class TestAttemptListItemModel
    {
        public long TestAttemptId { get; set; }
        public long TestName { get; set; }
        public int Duration { get; set; }
        public short? RawScore { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
