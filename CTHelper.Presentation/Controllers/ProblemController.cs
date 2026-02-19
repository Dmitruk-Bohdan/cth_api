using Microsoft.AspNetCore.Mvc;
using CTHelper.Application.Models.Dtos.ProblemDtos;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("problems")]
public class ProblemsController : ControllerBase
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
    public IActionResult Create([FromBody] CreateProblemRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id}")]
    public IActionResult Update(long id, [FromBody] UpdateProblemRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("{id}/topic/{topicId}")]
    public IActionResult UpdateTopic(long id, long topicId)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("{id}/difficulty/{difficulty}")]
    public IActionResult UpdateDifficulty(long id, int difficulty)
    {
        throw new NotImplementedException();
    }

    [HttpGet("my")]
    public IActionResult GetMyProblems()
    {
        throw new NotImplementedException();
    }
}
