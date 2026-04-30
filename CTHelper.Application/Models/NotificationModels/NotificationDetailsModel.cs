using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.Problem
{
    public class NotificationDetailsModel
    {
        public NotificationPriorityLevelTypeEnum PriorityLevel { get; set; }
        public string Payload { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
