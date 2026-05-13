using CTHelper.Application.Models;
using CTHelper.Application.Models.Notification;
using CTHelper.Application.Models.Problem;
using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Services.Interfaces
{
    public interface INotificationService
    {
        Task<OperationResult<PaginatedListResponseModel<NotificationListItemModel>>> GetMyNotificationList(long userId, int pageSize = 10, int pageNumber = 1);
        Task<OperationResult<NotificationDetailsModel>> GetNotificationDetails(NotificationDetailsRequestModel requestModel);
        Task<OperationResult> MarkAsRead(ReadNotificationRequestModel requestModel);
        Task<OperationResult> RemoveNotifications(RemoveNotificationRequestModel requestModel);
    }
}