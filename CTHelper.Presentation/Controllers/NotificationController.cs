using CTHelper.Application.Models;
using CTHelper.Application.Models.Group;
using CTHelper.Application.Models.Notification;
using CTHelper.Application.Models.Problem;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Common.Extensions;
using CTHelper.Domain.Entities;
using CTHelper.Infrastructure.Services.Implementations;
using CTHelper.Presentation.Dtos;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Claims;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("notifications")]
public class NotificationsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService, IMapper mapper)
    {
        _notificationService = notificationService;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetListAsync()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        OperationResult<PaginatedListResponseModel<NotificationListItemModel>> result = await _notificationService.GetMyNotificationList(userId);

        if (result.IsSuccess)
        {
            return Ok(result.Payload);
        }
        else
        {
            var errorDto = _mapper.Map<ErrorResponseDto>(result);
            return StatusCode(
                result.HttpStatusCode.ToInt(),
                errorDto);
        }
    }

    [HttpGet("{notificationId:long}")]
    [Authorize]
    public async Task<IActionResult> GetByIdAsync(long notificationId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new NotificationDetailsRequestModel()
        {
            UserId = userId,
            NotificationId = notificationId
        };

        OperationResult<NotificationDetailsModel> result = await _notificationService.GetNotificationDetails(requestModel);

        if (result.IsSuccess)
        {
            return Ok(result.Payload);
        }
        else
        {
            var errorDto = _mapper.Map<ErrorResponseDto>(result);
            return StatusCode(
                result.HttpStatusCode.ToInt(),
                errorDto);
        }
    }

    [HttpDelete("remove/{notificationId:long}")]
    [Authorize]
    public async Task<IActionResult> RemoveAsync(long notificationId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new RemoveNotificationRequestModel()
        {
            UserId = userId,
            NotificationId = notificationId
        };

        OperationResult result = await _notificationService.RemoveNotification(requestModel);

        if (result.IsSuccess)
        {
            return Ok();
        }
        else
        {
            var errorDto = _mapper.Map<ErrorResponseDto>(result);
            return StatusCode(
                result.HttpStatusCode.ToInt(),
                errorDto);
        }
    }

    [HttpDelete("remove-all")]
    [Authorize]
    public async Task<IActionResult> RemoveAllAsync()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new RemoveAllNotificationRequestModel()
        {
            UserId = userId
        };

        OperationResult result = await _notificationService.RemoveAllNotification(requestModel);

        if (result.IsSuccess)
        {
            return Ok();
        }
        else
        {
            var errorDto = _mapper.Map<ErrorResponseDto>(result);
            return StatusCode(
                result.HttpStatusCode.ToInt(),
                errorDto);
        }
    }

    [HttpDelete("read/{notificationId:long}")]
    [Authorize]
    public async Task<IActionResult> MarkAsReadAsync(long notificationId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new ReadNotificationRequestModel()
        {
            UserId = userId,
            NotificationId = notificationId
        };

        OperationResult result = await _notificationService.MarkAsRead(requestModel);

        if (result.IsSuccess)
        {
            return Ok();
        }
        else
        {
            var errorDto = _mapper.Map<ErrorResponseDto>(result);
            return StatusCode(
                result.HttpStatusCode.ToInt(),
                errorDto);
        }
    }

    [HttpPatch("read-all")]
    [Authorize]
    public async Task<IActionResult> MarkAllAsReadAsync()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new ReadAllNotificationRequestModel()
        {
            UserId = userId
        };

        OperationResult result = await _notificationService.ReadAllNotification(requestModel);

        if (result.IsSuccess)
        {
            return Ok();
        }
        else
        {
            var errorDto = _mapper.Map<ErrorResponseDto>(result);
            return StatusCode(
                result.HttpStatusCode.ToInt(),
                errorDto);
        }
    }
}
