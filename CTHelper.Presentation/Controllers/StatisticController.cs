using Microsoft.AspNetCore.Mvc;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("statistics")]
public class StatisticsController : ControllerBase
{
    [HttpGet("me")]
    public IActionResult GetMyStatistics([FromQuery] long subjectId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("students/{id}")]
    public IActionResult GetStudentStatistics([FromRoute] long studentId, [FromQuery] long subjectId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("groups/{id}")]
    public IActionResult GetGroupStatistics([FromRoute] long groupId)
    {
        throw new NotImplementedException();
    }
}
