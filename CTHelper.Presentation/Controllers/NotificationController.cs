using Microsoft.AspNetCore.Mvc;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("notifications")]
public class NotificationsController : ControllerBase
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

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("read-all")]
    public IActionResult MarkAllAsRead()
    {
        throw new NotImplementedException();
    }
}
