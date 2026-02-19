using Microsoft.AspNetCore.Mvc;
using CTHelper.Application.Models.Dtos.GroupDtos;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("groups")]
public class GroupsController : ControllerBase
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
    public IActionResult Create([FromBody] CreateGroupRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id}")]
    public IActionResult Update(long id, [FromBody] UpdateGroupRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{id}/students/{studentId}")]
    public IActionResult AddStudent(long id, long studentId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}/students/{studentId}")]
    public IActionResult RemoveStudent(long id, long studentId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}/assignments")]
    public IActionResult GetAssignments(long id)
    {
        throw new NotImplementedException();
    }
}
