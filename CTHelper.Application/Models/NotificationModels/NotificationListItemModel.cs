using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.Notification
{
    public class NotificationListItemModel
    {
        public long NotificationId { get; set; }    
        public NotificationPriorityLevelTypeEnum PriorityLevel { get; set; }
        public string PayloadPreview { get; set; } = default!;
        public bool IsSeen { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
