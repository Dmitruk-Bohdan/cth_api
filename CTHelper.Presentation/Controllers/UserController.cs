using CTHelper.Application.UseCases.UserManagment.Command;
using CTHelper.Domain.Common.Extensions;
using CTHelper.Presentation.Dtos;
using CTHelper.Presentation.Dtos.UserDtos;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("user")]
public class UsersController : ControllerBase
{
    private IMapper _mapper;
    private IMediator _mediator;

    public UsersController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet("me/")]
    [Authorize]
    public async Task<IActionResult> GetMyInfo()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }
        var query = new GetUserInfoByIdQuery(userId);

        var result = await _mediator.Send(query);

        if (result.ErrorCode == null)
        {
            return Ok(result);
        }
        else
        {
            var errorDto = _mapper.Map<ErrorResponseDto>(result);
            return StatusCode(
                result.HttpStatusCode.ToInt(),
                errorDto);
        }
    }

    [HttpPut("me/")]
    [Authorize]
    public async Task<IActionResult> UpdateUserProfileAsync([FromForm] UpdateUserProfileRequestDto request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var updateUserProfileCommand = _mapper.Map<UpdateUserProfileCommand>(request);
        updateUserProfileCommand = updateUserProfileCommand with
        {
            UserId = userId
        };

        var result = await _mediator.Send(updateUserProfileCommand);

        if (result.ErrorCode == null)
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

    [HttpDelete("me/")]
    [Authorize]
    public async Task<IActionResult> DeleteUserAsync()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }
        
        var command = new DeleteUserCommand(userId);
        var result = await _mediator.Send(command);

        if (result.ErrorCode == null)
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

    [HttpPost("avatar/")]
    [Authorize]
    public async Task<IActionResult> UploadAvatar([FromForm] UploadAvatarDto request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        using var stream = request.AvatarFile.OpenReadStream();
        var command = new UpdateAvatarCommand(
            userId,
            request.AvatarFile.OpenReadStream(),
            request.AvatarFile.ContentType
        );

        var result = await _mediator.Send(command);

        if (result.ErrorCode == null)
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

    [HttpDelete("avatar/")]
    [Authorize]
    public async Task<IActionResult> DeleteAvatar()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var command = new DeleteAvatarCommand(userId);
        var result = await _mediator.Send(command);

        if (result.ErrorCode == null)
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
