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

        public async Task<OperationResult<PaginatedListResponseModel<NotificationListItemModel>>> GetMyNotificationList(long userId, int pageSize = 10, int pageNumber = 1)
        {
            var notificationsQuery = _dbContext.Notifications
                .Where(n => n.RecipientId == userId && !n.IsDeleted)
                .AsNoTracking();

            var notificationsCount = await notificationsQuery.CountAsync();
            var pagesCount = (int)Math.Ceiling((double)notificationsCount / pageSize);

            var notificationList = await notificationsQuery
                .OrderBy(n => n.IsSeen)
                .ThenBy(n => n.PriorityLevel)
                .ThenByDescending(n => n.CreatedAt) 
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(n => new NotificationListItemModel()
                {
                    NotificationId = n.Id,
                    PriorityLevel = n.PriorityLevel,
                    PayloadPreview = n.Payload.Length > 100 ? n.Payload.Substring(0, 100) : n.Payload,
                    IsSeen = n.IsSeen,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync();

            var paginatedList = new PaginatedListResponseModel<NotificationListItemModel>()
            {
                Items = notificationList,
                TotalPagesCount = pagesCount,
                Page = pageNumber,
                PageSize = pageSize,
                HasPreviousPage = pageNumber > 1,
                HasNextPage = pageNumber < pagesCount
            };

            return new OperationResult<PaginatedListResponseModel<NotificationListItemModel>>(paginatedList);
        }

        public async Task<OperationResult<NotificationDetailsModel>> GetNotificationDetails(NotificationDetailsRequestModel requestModel)
        {
            var notification = await _dbContext.Notifications
                .FirstOrDefaultAsync(n => n.Id == requestModel.NotificationId && !n.IsDeleted);

            if (notification == null)
            {
                return new OperationResult<NotificationDetailsModel>()
                {
                    ErrorCode = ErrorCodeConstants.NotificationNotFound,
                    ErrorMessage = "Notification not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            if (notification.RecipientId != requestModel.UserId)
            {
                return new OperationResult<NotificationDetailsModel>()
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You can only read and modify your own notifications",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            var notificationDetails = new NotificationDetailsModel()
            {
                NotificationId = notification.Id,
                PriorityLevel = notification.PriorityLevel,
                Payload = notification.Payload,
                CreatedAt = notification.CreatedAt
            };

            notification.IsSeen = true;
            await _dbContext.SaveChangesAsync();

            return new OperationResult<NotificationDetailsModel>(notificationDetails);
        }

        public async Task<OperationResult> MarkAsRead(ReadNotificationRequestModel requestModel)
        {
            if (requestModel.NotificationIds == null || !requestModel.NotificationIds.Any())
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.NotificationIdsListIsEmpty,
                    ErrorMessage = "Notification ids list is empty",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == requestModel.UserId);

            if (user == null)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.UserNotFound,
                    ErrorMessage = "User not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var notifications = await _dbContext.Notifications
                .Where(n => requestModel.NotificationIds.Contains(n.Id) && !n.IsDeleted)
                .ToListAsync();

            if (!notifications.Any())
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.NotificationNotFound,
                    ErrorMessage = "No notifications found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var foreignNotifications = notifications
                .Where(n => n.RecipientId != requestModel.UserId)
                .ToList();

            if (foreignNotifications.Any())
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You can only read and modify your own notifications",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            foreach (var notification in notifications)
            {
                notification.IsSeen = true;
            }

            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> RemoveNotifications(RemoveNotificationRequestModel requestModel)
        {
            if (requestModel.NotificationIds == null || !requestModel.NotificationIds.Any())
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.NotificationIdsListIsEmpty,
                    ErrorMessage = "Notification ids list is empty",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == requestModel.UserId);

            if (user == null)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.UserNotFound,
                    ErrorMessage = "User not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var notifications = await _dbContext.Notifications
                .Where(n => requestModel.NotificationIds.Contains(n.Id) && !n.IsDeleted)
                .ToListAsync();

            if (!notifications.Any())
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.NotificationNotFound,
                    ErrorMessage = "No notifications found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var foreignNotifications = notifications
                .Where(n => n.RecipientId != requestModel.UserId)
                .ToList();

            if (foreignNotifications.Any())
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You can only read and modify your own notifications",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            foreach (var notification in notifications)
            {
                notification.IsDeleted = true;
            }

            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }
    }
}