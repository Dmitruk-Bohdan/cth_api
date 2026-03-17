using Microsoft.AspNetCore.Mvc;
using CTHelper.Application.Models.Dtos.UserDtos;
using CTHelper.Application.Services.Interfaces;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private IFileStorageService _fileStorageService;

    public UsersController(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var list = await _fileStorageService.GetFileNamesAsync();
        return Ok(list);
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
