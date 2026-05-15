using CTHelper.Application.Models;
using CTHelper.Application.Models.Assignment;
using CTHelper.Application.Models.AssignmentModels;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Common.Extensions;
using CTHelper.Presentation.Common.Constants;
using CTHelper.Presentation.Dtos;
using CTHelper.Presentation.Dtos.AssignmentDtos;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("assignments")]
public class AssignmentsController : BaseController
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentsController(IAssignmentService assignmentService, IMapper mapper) : base(mapper)
    {
        _assignmentService = assignmentService;
    }

    [HttpPost("students")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AssignTestToStudent([FromBody] AssignTestToStudentRequestDto dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new AssignTestToStudentRequestModel()
        {
            TestId = dto.TestId,
            StudentId = dto.StudentId,
            Deadline = dto.Deadline,
            AttemptsAllowed = dto.AttemptsAllowed,
            TeacherId = userId
        };

        var result = await _assignmentService.AssignTestToStudent(requestModel);

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

    [HttpPost("groups")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AssignTestToGroup([FromBody] AssignTestToGroupRequestDto dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new AssignTestToGroupRequestModel()
        {
            TestId = dto.TestId,
            GroupId = dto.GroupId,
            Deadline = dto.Deadline,
            AttemptsAllowed = dto.AttemptsAllowed,
            TeacherId = userId
        };

        var result = await _assignmentService.AssignTestToGroup(requestModel);

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

    [HttpPatch("")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> PatchAssignment([FromBody] PatchAssignmentRequestDto dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new PatchAssignmentRequestModel()
        {
            TeacherId = userId,
            AssignmentId = dto.AssignmentId,
            Deadline = dto.Deadline,
            Attempts = dto.Attempts,
        };

        var result = await _assignmentService.PatchAssignment(requestModel);

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

    [HttpDelete("")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RevokeAssignment([FromBody] RevokeAssignmentRequestDto dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new RevokeAssignmentRequestModel()
        {
            TeacherId = userId,
            AssignmentId = dto.AssignmentId,
        };

        var result = await _assignmentService.RevokeAssignment(requestModel);

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

    [HttpGet("teacher/me")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(typeof(PaginatedListResponseModel<TeacherAssignmentPreviewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIAssignedList([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new GetIAssignedRequestModel()
        {
            UserId = userId,
            PageSize = pageSize,
            PageNumber = pageNumber
        };

        var result = await _assignmentService.GetIAssignedList(requestModel);

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

    [HttpGet("student/me")]
    [Authorize(Policy = PoliciesNamesConstants.StudentOnlyPolicy)]
    [ProducesResponseType(typeof(PaginatedListResponseModel<StudentAssignmentPreviewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAssignedToMeList([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new GetAssignedToMeRequestModel()
        {
            UserId = userId,
            PageSize = pageSize,
            PageNumber = pageNumber
        };

        var result = await _assignmentService.GetAssignedToMeList(requestModel);

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

    [HttpGet("student/details/{assignmentId:long}")]
    [Authorize]
    [ProducesResponseType(typeof(StudentAssignmentDetailsModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentAssignmentDetails([FromRoute] long assignmentId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var isStudent = User.IsInRole("Student");
        var isTeacher = User.IsInRole("Teacher");

        var requestModel = new GetAssignmentDetailsModel()
        {
            AssignmentId = assignmentId,
            StudentId = isStudent ? userId : null,
            TeacherId = isTeacher ? userId : null
        };

        var result = await _assignmentService.GetStudentAssignmentDetails(requestModel);

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

    [HttpGet("student/{studentId:long}/list")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(typeof(PaginatedListResponseModel<StudentAssignmentPreviewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentAssignments([FromRoute] long studentId, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new GetAssignedToStudentListModel()
        {
            StudentId = studentId,
            TeacherId = userId,
            PageSize = pageSize,
            PageNumber = pageNumber
        };

        var result = await _assignmentService.GetAssignedToStudentList(requestModel);

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
    [HttpGet("group/details/{assignmentId:long}")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(typeof(GroupAssignmentDetailsModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGroupAssignmentsDetails([FromRoute] long assignmentId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new GetAssignmentDetailsModel()
        {
            AssignmentId = assignmentId,
            TeacherId = userId
        };

        var result = await _assignmentService.GetGroupAssignmentDetails(requestModel);

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

    [HttpGet("group/{groupId:long}/list")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(typeof(PaginatedListResponseModel<StudentAssignmentPreviewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGroupAssignmentList([FromRoute] long groupId, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new GetAssignedToGroupListModel()
        {
            GroupId = groupId,
            TeacherId = userId,
            PageSize = pageSize,
            PageNumber = pageNumber
        };

        var result = await _assignmentService.GetAssignedToGroupList(requestModel);

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

    [HttpGet("group-score/{assignmentId:long}")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(typeof(GroupScoreByAssignmentResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGroupAssignmentScore([FromRoute] long assignmentId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new GetGroupAssignmentScoreModel()
        {
            AssignmentId = assignmentId,
            TeacherId = userId
        };

        var result = await _assignmentService.GetGroupAssignmentScore(requestModel);

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

    [HttpGet("student-score/{assignmentId:long}")]
    [Authorize]
    [ProducesResponseType(typeof(StudentScoreByAssignmentResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentAssignmentScore([FromRoute] long assignmentId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var isStudent = User.IsInRole("Student");
        var isTeacher = User.IsInRole("Teacher");

        var requestModel = new GetStudentAssignmentScoreModel()
        {
            AssignmentId = assignmentId,
            StudentId = isStudent ? userId : null,
            TeacherId = isTeacher ? userId : null
        };

        var result = await _assignmentService.GetStudentAssignmentScore(requestModel);

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
}