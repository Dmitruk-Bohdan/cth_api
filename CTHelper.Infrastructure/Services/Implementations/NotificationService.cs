using CTHelper.Application.Models;
using CTHelper.Application.Models.Notification;
using CTHelper.Application.Models.Problem;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Presentation.Dtos;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        public Task<OperationResult<PaginatedListResponseModel<NotificationPreviewModel>>> GetMyNotificationList(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<NotificationDetailsModel>> GetNotificationDetails(NotificationDetailsRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> MarkAsRead(ReadNotificationRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> ReadAllNotification(ReadAllNotificationRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> RemoveAllNotification(RemoveAllNotificationRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> RemoveNotification(RemoveNotificationRequestModel requestModel)
        {
            throw new NotImplementedException();
        }
    }
}
