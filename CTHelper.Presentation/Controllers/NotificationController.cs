using CTHelper.Application.Models;
using CTHelper.Application.Models.Notification;
using CTHelper.Application.Models.Problem;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Common.Extensions;
using CTHelper.Presentation.Dtos;
using CTHelper.Presentation.Dtos.NotificationDtos;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("list")]
    [Authorize]
    [ProducesResponseType(typeof(PaginatedListResponseModel<NotificationListItemModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListAsync([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        OperationResult<PaginatedListResponseModel<NotificationListItemModel>> result = await _notificationService.GetMyNotificationList(userId, pageSize, pageNumber);

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
    [ProducesResponseType(typeof(NotificationDetailsModel), StatusCodes.Status200OK)]
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

    [HttpDelete("remove")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveAsync([FromBody] RemoveNotificationRequestDto requestDto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new RemoveNotificationRequestModel()
        {
            UserId = userId,
            NotificationIds = requestDto.NotificationIds
        };

        OperationResult result = await _notificationService.RemoveNotifications(requestModel);

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

    [HttpPatch("read")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> MarkAsReadAsync([FromBody] ReadNotificationRequestDto requestDto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new ReadNotificationRequestModel()
        {
            UserId = userId,
            NotificationIds = requestDto.NotificationIds
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
}