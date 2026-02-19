using Microsoft.AspNetCore.Mvc;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("favourites")]
public class FavouritesController : ControllerBase
{
    [HttpGet("problems")]
    public IActionResult GetProblemFavourites()
    {
        throw new NotImplementedException();
    }

    [HttpPost("problems/{problemId}")]
    public IActionResult AddProblemFavourite(long problemId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("problems/{problemId}")]
    public IActionResult RemoveProblemFavourite(long problemId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("tests")]
    public IActionResult GetTestFavourites()
    {
        throw new NotImplementedException();
    }

    [HttpPost("tests/{testId}")]
    public IActionResult AddTestFavourite(long testId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("tests/{testId}")]
    public IActionResult RemoveTestFavourite(long testId)
    {
        throw new NotImplementedException();
    }
}
