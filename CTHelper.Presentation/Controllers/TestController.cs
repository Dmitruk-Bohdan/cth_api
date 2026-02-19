using Microsoft.AspNetCore.Mvc;
using CTHelper.Application.Models.Dtos.TestDtos;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("tests")]
public class TestsController : ControllerBase
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
    public IActionResult Create([FromBody] CreateTestRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id}")]
    public IActionResult Update(long id, [FromBody] UpdateTestRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        throw new NotImplementedException();
    }

    [HttpPost("mixed")]
    public IActionResult CreateMixedTest([FromBody] CreateTestRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpGet("my")]
    public IActionResult GetMyTests()
    {
        throw new NotImplementedException();
    }
}
