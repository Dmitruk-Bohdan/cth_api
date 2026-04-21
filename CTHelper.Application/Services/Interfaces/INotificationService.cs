using CTHelper.Application.Models;
using CTHelper.Application.Models.Notification;
using CTHelper.Application.Models.Problem;
using CTHelper.Presentation.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTHelper.Application.Services.Interfaces
{
    public interface INotificationService
    {
        Task<OperationResult<PaginatedListResponseModel<NotificationListItemModel>>> GetMyNotificationList(long userId);
        Task<OperationResult<NotificationDetailsModel>> GetNotificationDetails(NotificationDetailsRequestModel requestModel);
        Task<OperationResult> MarkAsRead(ReadNotificationRequestModel requestModel);
        Task<OperationResult> ReadAllNotification(ReadAllNotificationRequestModel requestModel);
        Task<OperationResult> RemoveAllNotification(RemoveAllNotificationRequestModel requestModel);
        Task<OperationResult> RemoveNotification(RemoveNotificationRequestModel requestModel);
    }
}
