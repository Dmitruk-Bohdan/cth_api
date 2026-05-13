namespace CTHelper.Application.Models.Notification
{
    public class ReadNotificationRequestModel
    {
        public long UserId { get; set; }
        public List<long> NotificationIds { get; set; } = new();
    }
}
