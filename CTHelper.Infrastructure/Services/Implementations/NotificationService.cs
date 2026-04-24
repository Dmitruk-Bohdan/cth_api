using CTHelper.Application.Common.Constants;
using CTHelper.Application.Models;
using CTHelper.Application.Models.Notification;
using CTHelper.Application.Models.Problem;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Persistence.Context;
using CTHelper.Presentation.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _dbContext;

        public NotificationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PaginatedListResponseModel<NotificationListItemModel>>> GetMyNotificationList(long userId)
        {
            var notificationsQuery = _dbContext.Notifications
                .Where(n => n.RecipientId == userId)
                .AsNoTracking();

            var notificationsCount = await notificationsQuery.CountAsync();

            var notificationList = await notificationsQuery
                .Select(n => new NotificationListItemModel()
                {
                    PriorityLevel = n.PriorityLevel,
                    PayloadPreview = n.Payload.Length > 100 ? n.Payload.Substring(0, 100) : n.Payload,
                    IsSeen = n.IsSeen,
                    CreatedAt = n.CreatedAt
                })
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            var paginatedList = new PaginatedListResponseModel<NotificationListItemModel>()
            {
                Items = notificationList,
                TotalPagesCount = 1,
                Page = 1,
                PageSize = notificationsCount,
                HasPreviousPage = false,
                HasNextPage = false
            };

            return new OperationResult<PaginatedListResponseModel<NotificationListItemModel>>(paginatedList);
        }

        public async Task<OperationResult<NotificationDetailsModel>> GetNotificationDetails(NotificationDetailsRequestModel requestModel)
        {
            var notification = await _dbContext.Notifications
                .Where(n =>
                    n.Id == requestModel.NotificationId
                    && n.RecipientId == requestModel.UserId)
                .AsNoTracking()
                .Select(n => new NotificationDetailsModel()
                {
                    PriorityLevel = n.PriorityLevel,
                    Payload = n.Payload,
                    CreatedAt = n.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (notification == null)
            {
                return new OperationResult<NotificationDetailsModel>()
                {
                    ErrorCode = ErrorCodeConstants.NotificationNotFound,
                    ErrorMessage = "Notification not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            return new OperationResult<NotificationDetailsModel>(notification);
        }

        public async Task<OperationResult> MarkAsRead(ReadNotificationRequestModel requestModel)
        {
            var notification = await _dbContext.Notifications
                .Where(n =>
                    n.Id == requestModel.NotificationId
                    && n.RecipientId == requestModel.UserId)
                .FirstOrDefaultAsync();

            if (notification == null)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.NotificationNotFound,
                    ErrorMessage = "Notification not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            notification.IsSeen = true;
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> ReadAllNotification(ReadAllNotificationRequestModel requestModel)
        {
            var notifications = await _dbContext.Notifications
                .Where(n =>
                    n.RecipientId == requestModel.UserId
                    && !n.IsSeen)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsSeen = true;
            }

            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> RemoveAllNotification(RemoveAllNotificationRequestModel requestModel)
        {
            var notifications = await _dbContext.Notifications
                .Where(n => n.RecipientId == requestModel.UserId)
                .ToListAsync();

            _dbContext.Notifications.RemoveRange(notifications);
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> RemoveNotification(RemoveNotificationRequestModel requestModel)
        {
            var notification = await _dbContext.Notifications
                .Where(n =>
                    n.Id == requestModel.NotificationId
                    && n.RecipientId == requestModel.UserId)
                .FirstOrDefaultAsync();

            if (notification == null)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.NotificationNotFound,
                    ErrorMessage = "Notification not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            _dbContext.Notifications.Remove(notification);
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }
    }
}
