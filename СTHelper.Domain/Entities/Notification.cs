using СTHelper.Domain.Common.Enums;

namespace СTHelper.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public long RecipientId { get; set; }

        public NotificationPriorityLevelType PriorityLevel { get; set; }

        public string Payload { get; set; } = default!;

        public bool IsSeen { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public User Recipient { get; set; } = default!;
    }
}
