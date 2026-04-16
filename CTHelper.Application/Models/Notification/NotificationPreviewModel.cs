using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.Notification
{
    public class NotificationPreviewModel
    {
        public NotificationPriorityLevelType PriorityLevel { get; set; }
        public string PayloadPreview { get; set; } = default!;
        public bool IsSeen { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
