using Microsoft.AspNetCore.Mvc;
using CTHelper.Application.Models.Dtos.TestAttemptDtos;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("attempts")]
public class TestAttemptsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}")]
    public IActionResult GetById(long id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public IActionResult Start([FromBody] StartTestAttemptRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("{id}/pause")]
    public IActionResult Pause(long id)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("{id}/resume")]
    public IActionResult Resume(long id)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("{id}/complete")]
    public IActionResult Complete(long id)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("{id}/cancel")]
    public IActionResult Cancel(long id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}/answers")]
    public IActionResult GetAnswers(long id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("my")]
    public IActionResult GetMyAttempts()
    {
        throw new NotImplementedException();
    }
}
