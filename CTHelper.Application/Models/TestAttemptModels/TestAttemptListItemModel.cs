using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.TestAttemptModels
{
    public class TestAttemptListItemModel
    {
        public long TestAttemptId { get; set; }
        public TestAttemptStatusTypeEnum Status { get; set; }
        public string TestName { get; set; } = default!;
        public int Duration { get; set; }
        public short? RawScore { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
