using CTHelper.Application.Models.Dtos;
using CTHelper.Application.Models.Dtos.AssignmentDtos;
using Microsoft.AspNetCore.Mvc;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("assignments")]
public class AssignmentsController : ControllerBase
{
    [HttpGet]
    [Route("i-assigned")]
    public IActionResult GetAllMyAssignments()
    {
        return Ok(new TeacherAssignmentListResponseDto());
    }

    [HttpGet]
    [Route("assigned-me")]
    public IActionResult GetAllAssignedMe()
    {
        return Ok(new StudentAssignmentListResponseDto());
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] long id)
    {
        var role = "role";
        if(role == "teacher")
        {
            return Ok(new TeacherAssignmentItemResponseModel());
        }
        else
        {
            return Ok(new StudentAssignmentItemResponseModel());
        }
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateAssignmentRequestDto dto)
    {
        var createdAssignment = new StudentAssignmentItemResponseModel();
        return CreatedAtAction(
            nameof(GetById),
            new IdDto(createdAssignment.Id));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] long id)
    {
        return NoContent();
    }

    [HttpPatch("{id}/deadline")]
    public IActionResult ChangeDeadline(
        [FromRoute] long id,
        [FromBody] PatchAssignmentDeadlineRequestDto dto)
    {
        return NoContent();
    }

    [HttpGet("by-group-score/{assignmentId}")]
    public IActionResult GetByGroup(long assignmentId)
    {
        return Ok(new GroupScoreByAssignmentResponseDto());
    }

    [HttpGet("by-student-score/{studentId}")]
    public IActionResult GetByStudent(long studentId)
    {
        return Ok(new TeacherAssignmentListResponseDto());
    }
}
