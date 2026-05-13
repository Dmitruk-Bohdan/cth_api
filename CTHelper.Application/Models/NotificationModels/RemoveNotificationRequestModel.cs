namespace CTHelper.Application.Models.Notification
{
    public class RemoveNotificationRequestModel
    {
        public long UserId { get; set; }
        public List<long> NotificationIds { get; set; } = new();
    }
}
