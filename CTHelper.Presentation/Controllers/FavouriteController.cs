using CTHelper.Application.Models;
using CTHelper.Application.Models.Favourite;
using CTHelper.Application.Models.Problem;
using CTHelper.Application.Models.TestModels;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Common.Extensions;
using CTHelper.Presentation.Dtos;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("favourites")]
public class FavouritesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IFavouriteService _favouriteService;

    public FavouritesController(IFavouriteService favouriteService, IMapper mapper)
    {
        _favouriteService = favouriteService;
        _mapper = mapper;
    }

    [HttpGet("problems")]
    [Authorize]
    [ProducesResponseType(typeof(PaginatedListResponseModel<ProblemListItemModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFavouriteProblemListAsync([FromQuery] long subjectId, [FromQuery] string? searchTerm, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new MyFavouriteProblemListRequestModel()
        {
            UserId = userId,
            SearchTerm = searchTerm,
            SubjectId = subjectId,
            PageSize = pageSize,
            PageNumber = pageNumber
        };

        OperationResult<PaginatedListResponseModel<ProblemListItemModel>> result = await _favouriteService.GetMyFavouriteProblemList(requestModel);

        if (result.IsSuccess)
        {
            return Ok(result.Payload);
        }
        else
        {
            var errorDto = _mapper.Map<ErrorResponseDto>(result);
            return StatusCode(
                result.HttpStatusCode.ToInt(),
                errorDto);
        }
    }

    [HttpPost("problems/{problemId:long}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddProblemToFavourite([FromRoute] long problemId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new AddProblemToFavouriteRequestModel()
        {
            UserId = userId,
            ProblemId = problemId
        };

        OperationResult result = await _favouriteService.AddProblemToFavourite(requestModel);

        if (result.IsSuccess)
        {
            return Ok();
        }
        else
        {
            var errorDto = _mapper.Map<ErrorResponseDto>(result);
            return StatusCode(
                result.HttpStatusCode.ToInt(),
                errorDto);
        }
    }

    [HttpDelete("problems/{problemId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveProblemFromFavouriteAsync([FromRoute] long problemId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new RemoveProblemFromFavouriteRequestModel()
        {
            UserId = userId,
            ProblemId = problemId
        };

        OperationResult result = await _favouriteService.RemoveProblemFromFavourite(requestModel);

        if (result.IsSuccess)
        {
            return Ok();
        }
        else
        {
            var errorDto = _mapper.Map<ErrorResponseDto>(result);
            return StatusCode(
                result.HttpStatusCode.ToInt(),
                errorDto);
        }
    }

    [HttpGet("tests")]
    [Authorize]
    [ProducesResponseType(typeof(PaginatedListResponseModel<TestPreviewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTestFavouritesAsync([FromQuery] long subjectId, [FromQuery] string? searchTerm, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new MyFavouriteTestListRequestModel()
        {
            UserId = userId,
            SearchTerm = searchTerm,
            SubjectId = subjectId,
            PageSize = pageSize,
            PageNumber = pageNumber
        };

        OperationResult<PaginatedListResponseModel<TestPreviewModel>> result = await _favouriteService.GetMyFavouriteTestList(requestModel);

        if (result.IsSuccess)
        {
            return Ok(result.Payload);
        }
        else
        {
            var errorDto = _mapper.Map<ErrorResponseDto>(result);
            return StatusCode(
                result.HttpStatusCode.ToInt(),
                errorDto);
        }
    }

    [HttpPost("tests/{testId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddTestFavouriteAsync([FromRoute] long testId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new AddTestToFavouriteRequestModel()
        {
            UserId = userId,
            TestId = testId
        };

        OperationResult result = await _favouriteService.AddTestToFavourite(requestModel);

        if (result.IsSuccess)
        {
            return Ok();
        }
        else
        {
            var errorDto = _mapper.Map<ErrorResponseDto>(result);
            return StatusCode(
                result.HttpStatusCode.ToInt(),
                errorDto);
        }
    }

    [HttpDelete("tests/{testId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveTestFavouriteAsync([FromRoute] long testId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new RemoveTestFromFavouriteRequestModel()
        {
            UserId = userId,
            TestId = testId
        };

        OperationResult result = await _favouriteService.RemoveTestFromFavourite(requestModel);

        if (result.IsSuccess)
        {
            return Ok();
        }
        else
        {
            var errorDto = _mapper.Map<ErrorResponseDto>(result);
            return StatusCode(
                result.HttpStatusCode.ToInt(),
                errorDto);
        }
    }
}
