using CTHelper.Domain.Entities;
using CTHelper.Persistence.Common.Enums;

namespace CTHelper.Persistence.Entities
{
    public class UserEvent
    {
        public long Id { get; set; }
        public long UserId { get; set; }  
        public UserEventTypeEnum EventType { get; set; }
        public long? TargetId { get; set; }
        public string? IpAddress { get; set; } 
        public string? DeviceInfo { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public User User { get; set; } = default!;
    }
}
