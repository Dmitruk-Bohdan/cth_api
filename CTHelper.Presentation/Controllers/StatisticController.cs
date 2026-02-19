using Microsoft.AspNetCore.Mvc;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("statistics")]
public class StatisticsController : ControllerBase
{
    [HttpGet("me")]
    public IActionResult GetMyStatistics()
    {
        throw new NotImplementedException();
    }

    [HttpGet("students/{id}")]
    public IActionResult GetStudentStatistics(long id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("groups/{id}")]
    public IActionResult GetGroupStatistics(long id)
    {
        throw new NotImplementedException();
    }
}
