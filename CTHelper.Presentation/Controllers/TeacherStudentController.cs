using CTHelper.Application.Models.TeacherStudent;
using CTHelper.Application.Models.UserModels;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Application.UseCases.TeacherStudentRelationship.Command;
using CTHelper.Domain.Common.Extensions;
using CTHelper.Presentation.Common.Constants;
using CTHelper.Presentation.Dtos;
using CTHelper.Presentation.Dtos.TeacherStudentDtos;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("teacher-student")]
public class TeacherStudentController : ControllerBase
{
    private readonly ITeacherStudentService _teacherStudentService;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;


    public TeacherStudentController(IMediator mediator, IMapper mapper, ITeacherStudentService teacherStudentService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _teacherStudentService = teacherStudentService;
    }

    [HttpPost("invitation-code")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(typeof(CreateInvitationCodeResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateInvitationCode([FromBody] CreateInvitationCodeRequestDto request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var createInvitationCodeCommand = _mapper.Map<CreateInvitationCodeCommand>(request);
        createInvitationCodeCommand = createInvitationCodeCommand with
        {
            TeacherId = userId
        };

        var result = await _mediator.Send(createInvitationCodeCommand);

        if (result.ErrorCode == null)
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

    [HttpPost("binding/request")]
    [Authorize(Policy = PoliciesNamesConstants.StudentOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task <IActionResult> RequestBindingWithTeacherByCode([FromBody] CreateBindingRequestDto request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _teacherStudentService.RequestBindingWithTeacherByCode(userId, request.Code);

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

    [HttpPost("binding/accept")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AcceptStudentByInvitationCode([FromBody] AcceptBindingRequestDto request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _teacherStudentService.AcceptStudentByInvitationCode(userId, request.BindingRequestId);

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

    // =======================
    // BINDING MANAGEMENT
    // =======================

    [HttpDelete("binding/remove-teacher/{bindingId:long}")]
    [Authorize(Policy = PoliciesNamesConstants.StudentOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveBindingWithTeacher(long bindingId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _teacherStudentService.RemoveBindingWithTeacher(userId, bindingId);

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

    [HttpDelete("binding/remove-student/{bindingId:long}")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveBindingWithStudent(long bindingId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _teacherStudentService.RemoveBindingWithStudent(userId, bindingId);

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

    // =======================
    // BLOCK / UNBLOCK
    // =======================

    [HttpPost("binding/{studentId:long}/block")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> BlockStudentAsync(long studentId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _teacherStudentService.BlockStudent(userId, studentId);

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

    [HttpPost("binding/{bindingId:long}/unblock")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UnblockStudentAsync(long bindingId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _teacherStudentService.UnblockStudent(userId, bindingId);

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

    // =======================
    // GET MY STUDENTS
    // =======================

    [HttpGet("students/{studentId:long}")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(typeof(UserProfileResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyStudentInfoByIdAsync(long studentId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _teacherStudentService.GetMyStudentInfoById(userId, studentId);

        if (result.ErrorCode == null)
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

    [HttpGet("students")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(typeof(List<UserProfilePreviewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyStudentsListAsync()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _teacherStudentService.GetMyStudentsList(userId);

        if (result.ErrorCode == null)
        {
            return Ok(new ListResponseDto<UserProfilePreviewModel>(result.Payload));
        }
        else
        {
            var errorDto = _mapper.Map<ErrorResponseDto>(result);
            return StatusCode(
                result.HttpStatusCode.ToInt(),
                errorDto);
        }
    }

    [HttpGet("students/blocked")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(typeof(List<UserProfilePreviewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBlockedStudentListAsync()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _teacherStudentService.GetBlockedStudentList(userId);

        if (result.ErrorCode == null)
        {
            return Ok(new ListResponseDto<UserProfilePreviewModel>(result.Payload));
        }
        else
        {
            var errorDto = _mapper.Map<ErrorResponseDto>(result);
            return StatusCode(
                result.HttpStatusCode.ToInt(),
                errorDto);
        }
    }

    // =======================
    // GET MY TEACHERS
    // =======================

    [HttpGet("teachers/{teacherId:long}")]
    [Authorize(Policy = PoliciesNamesConstants.StudentOnlyPolicy)]
    [ProducesResponseType(typeof(UserProfileResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyTeacherByIdAsync(long teacherId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _teacherStudentService.GetMyTeacherInfoById(userId, teacherId);

        if (result.ErrorCode == null)
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

    [HttpGet("teachers")]
    [Authorize(Policy = PoliciesNamesConstants.StudentOnlyPolicy)]
    [ProducesResponseType(typeof(List<UserProfilePreviewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyTeachersListAsync()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _teacherStudentService.GetMyTeachersList(userId);

        if (result.ErrorCode == null)
        {
            return Ok(new ListResponseDto<UserProfilePreviewModel>(result.Payload));
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