using Microsoft.AspNetCore.Mvc;
using CTHelper.Application.Models.Dtos.TeacherStudentDtos;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("bindings")]
public class TeacherStudentController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateBinding([FromBody] CreateBindingRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpPost("accept/{bindingId}")]
    public IActionResult AcceptBinding(long bindingId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBinding(long id)
    {
        throw new NotImplementedException();
    }

    [HttpPost("block/{bindingId}")]
    public IActionResult BlockBinding(long bindingId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("unblock/{bindingId}")]
    public IActionResult UnblockBinding(long bindingId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("students/{id}")]
    public IActionResult GetStudent(long id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("students")]
    public IActionResult GetStudents()
    {
        throw new NotImplementedException();
    }

    [HttpGet("students/blocked")]
    public IActionResult GetBlockedStudents()
    {
        throw new NotImplementedException();
    }

    [HttpGet("teachers/{id}")]
    public IActionResult GetTeacher(long id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("teachers")]
    public IActionResult GetTeachers()
    {
        throw new NotImplementedException();
    }
}
