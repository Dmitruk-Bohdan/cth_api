using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Application.UseCases.TeacherStudentRelationship.Command;
using CTHelper.Domain.Common.Extensions;
using CTHelper.Infrastructure.Settings;
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
[Route("api/teacher-student")]
public class TeacherStudentController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;


    public TeacherStudentController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }


    // =======================
    // INVITATION / BINDING FLOW
    // =======================

    [HttpPost("invitation-code")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
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
        
        throw new NotImplementedException();
    }

    [HttpPost("binding/request")]
    [Authorize(Policy = PoliciesNamesConstants.StudentOnlyPolicy)]
    public IActionResult RequestBindingWithTeacherByCode([FromBody] CreateBindingRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpPost("binding/accept")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    public IActionResult AcceptStudentByInvitationCode([FromBody] AcceptBindingRequestDto request)
    {
        throw new NotImplementedException();
    }

    // =======================
    // BINDING MANAGEMENT
    // =======================

    [HttpDelete("binding/{bindingId:long}")]
    [Authorize(Policy = PoliciesNamesConstants.StudentOnlyPolicy)]
    public IActionResult RemoveBindingWithTeacher(long bindingId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("student/{studentId:long}")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    public IActionResult RemoveStudentBinding(long studentId)
    {
        throw new NotImplementedException();
    }

    // =======================
    // BLOCK / UNBLOCK
    // =======================

    [HttpPost("binding/{bindingId:long}/block")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    public IActionResult BlockStudent(long bindingId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("binding/{bindingId:long}/unblock")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    public IActionResult UnblockStudent(long bindingId)
    {
        throw new NotImplementedException();
    }

    // =======================
    // GET MY STUDENTS
    // =======================

    [HttpGet("students/{id:long}")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    public IActionResult GetMyStudentById(long id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("students")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    public IActionResult GetMyStudentsList()
    {
        throw new NotImplementedException();
    }

    [HttpGet("students/blocked")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    public IActionResult GetBlockedStudentList()
    {
        throw new NotImplementedException();
    }

    // =======================
    // GET MY TEACHERS
    // =======================

    [HttpGet("teachers/{id:long}")]
    [Authorize(Policy = PoliciesNamesConstants.StudentOnlyPolicy)]
    public IActionResult GetMyTeacherById(long id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("teachers")]
    [Authorize(Policy = PoliciesNamesConstants.StudentOnlyPolicy)]
    public IActionResult GetMyTeachersList()
    {
        throw new NotImplementedException();
    }
}