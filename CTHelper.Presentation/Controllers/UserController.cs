using Microsoft.AspNetCore.Mvc;
using CTHelper.Application.Models.Dtos.UserDtos;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{

    [HttpGet("{id}")]
    public IActionResult GetById(long id)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id}")]
    public IActionResult Update(long id, [FromBody] UpdateUserRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("{id}/role")]
    public IActionResult UpdateRole(long id, [FromBody] UpdateUserRoleRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{id}/avatar")]
    public IActionResult UploadAvatar(long id)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id}/avatar")]
    public IActionResult UpdateAvatar(long id)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}/avatar")]
    public IActionResult DeleteAvatar(long id)
    {
        throw new NotImplementedException();
    }
}
