using CTHelper.Application.Models.Group;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Common.Extensions;
using CTHelper.Presentation.Common.Constants;
using CTHelper.Presentation.Dtos;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("groups")]
[Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
public class GroupsController : ControllerBase
{
    private readonly IGroupService _groupService;
    private readonly IMapper _mapper;

    public GroupsController(IGroupService groupService, IMapper mapper)
    {
        _groupService = groupService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyGroupList([FromQuery] MyGroupListRequestDto request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new MyGroupListRequestModel
        {
            TeacherId = userId,
            SubjectId = request.SubjectId,
            GroupName = request.GroupName,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        var result = await _groupService.GetMyGroupList(requestModel);

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

    [HttpGet("{groupId:long}")]
    public async Task<IActionResult> GetById([FromRoute] long groupId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new GetGroupByIdModel()
        {
            GroupId = groupId,
            TeacherId = userId,
        };

        var result = await _groupService.GetGroupById(requestModel);

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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGroupRequestDto request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var createGroupModel = new CreateGroupModel
        {
            TeacherId = userId,
            SubjectId = request.SubjectId,
            GroupName = request.GroupName
        };

        var result = await _groupService.CreateGroup(createGroupModel);

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

    [HttpDelete("{groupId:long}")]
    public async Task<IActionResult> Delete([FromRoute] long groupId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var deleteGroupModel = new DeleteGroupModel
        {
            TeacherId = userId,
            GroupId = groupId
        };

        var result = await _groupService.DeleteGroup(deleteGroupModel);

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

    [HttpPost("students")]
    public async Task<IActionResult> AddStudent([FromBody] AddStudentToGroupRequestDto request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var addStudentModel = new AddStudentToGroupModel
        {
            TeacherId = userId,
            StudentId = request.StudentId,
            GroupId = request.GroupId
        };

        var result = await _groupService.AddStudentToGroup(addStudentModel);

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

    [HttpDelete("students")]
    public async Task<IActionResult> RemoveStudent([FromBody] RemoveStudentFromGroupRequestDto requestDto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var removeStudentModel = new RemoveStudentFromGroupModel
        {
            TeacherId = userId,
            StudentId = requestDto.StudentId,
            GroupId = requestDto.GroupId
        };

        var result = await _groupService.RemoveStudentFromGroup(removeStudentModel);

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