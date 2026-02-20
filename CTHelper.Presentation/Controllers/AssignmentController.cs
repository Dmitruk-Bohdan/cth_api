using CTHelper.Application.Models.Dtos;
using CTHelper.Application.Models.Dtos.AssignmentDtos;
using Microsoft.AspNetCore.Mvc;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("assignments")]
public class AssignmentsController : ControllerBase
{
    /// <summary>
    /// Get all assignments for teacher or student
    /// </summary>
    /// <param name="subjectId">ID of the subject</param>
    /// <param name="notCompleted">Optional filter for incomplete assignments</param>
    /// <param name="searchTextFragment">Optional search by text</param>
    /// <param name="expiringDate">Optional filter by expiring date</param>
    /// <returns>List of assignments for teacher or student</returns>    
    [HttpGet("")]
    [ProducesResponseType(typeof(TeacherAssignmentListResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StudentAssignmentListResponseDto), StatusCodes.Status200OK)]
    public IActionResult GetAll(
        [FromQuery] long subjectId,
        [FromQuery] bool? notCompleted = null,
        [FromQuery] string? searchTextFragment = null,
        [FromQuery] DateTimeOffset? expiringDate = null)
    {
        var role = "role";
        if (role == "teacher")
        {
            return Ok(new TeacherAssignmentListResponseDto());
        }
        else
        {
            return Ok(new StudentAssignmentListResponseDto());
        }
    }

    /// <summary>
    /// Get details of a single assignment by its ID.
    /// </summary>
    /// <param name="id">ID of the assignment.</param>
    /// <returns>Assignment details. Response type depends on the role: teacher or student.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TeacherAssignmentItemResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StudentAssignmentItemResponseModel), StatusCodes.Status200OK)]
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

    /// <summary>
    /// Create a new assignment.
    /// </summary>
    /// <param name="dto">Assignment creation data.</param>
    /// <returns>Returns 201 Created with the ID of the newly created assignment.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(IdDto), StatusCodes.Status201Created)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]       
    public IActionResult Create([FromBody] CreateAssignmentRequestDto dto)
    {
        var createdAssignment = new StudentAssignmentItemResponseModel();

        return CreatedAtAction(
            nameof(GetById),
            new IdDto(createdAssignment.Id));
    }

    /// <summary>
    /// Delete an assignment by its ID.
    /// </summary>
    /// <param name="id">ID of the assignment to delete.</param>
    /// <returns>No content (204) on successful deletion.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]    
    public IActionResult Delete([FromRoute] long id)
    {
        return NoContent();
    }

    /// <summary>
    /// Change the deadline of an assignment.
    /// </summary>
    /// <param name="id">ID of the assignment to update.</param>
    /// <param name="dto">New deadline data.</param>
    /// <returns>No content (204) on successful update.</returns>
    [HttpPatch("{id}/deadline")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]    
    public IActionResult ChangeDeadline(
        [FromRoute] long id,
        [FromBody] PatchAssignmentDeadlineRequestDto dto)
    {
        return NoContent();
    }

    /// <summary>
    /// Get assignment scores grouped by student group.
    /// </summary>
    /// <param name="assignmentId">ID of the assignment.</param>
    /// <returns>Group scores for the specified assignment.</returns>
    [HttpGet("by-group-score/{assignmentId}")]
    [ProducesResponseType(typeof(GroupScoreByAssignmentResponseDto), StatusCodes.Status200OK)]  
    public IActionResult GetByGroup(long assignmentId)
    {
        return Ok(new GroupScoreByAssignmentResponseDto());
    }


    /// <summary>
    /// Get assignment scores for a specific student.
    /// </summary>
    /// <param name="studentId">ID of the student.</param>
    /// <returns>List of assignments with scores for the student (teacher view).</returns>
    [HttpGet("by-student-score/{studentId}")]
    [ProducesResponseType(typeof(TeacherAssignmentListResponseDto), StatusCodes.Status200OK)] 
    public IActionResult GetByStudent(long studentId)
    {
        return Ok(new TeacherAssignmentListResponseDto());
    }
}
