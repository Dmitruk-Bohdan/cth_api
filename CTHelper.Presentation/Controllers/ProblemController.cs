using CTHelper.Application.Models;
using CTHelper.Application.Models.Favourite;
using CTHelper.Application.Models.Problem;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Common.Extensions;
using CTHelper.Presentation.Dtos;
using CTHelper.Presentation.Dtos.ProblemDtos;
using MailKit.Search;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("problems")]
public class ProblemsController : ControllerBase
{
    private readonly IProblemService _problemService;
    private readonly IMapper _mapper;

    public ProblemsController(IMapper mapper, IProblemService problemService)
    {
        _mapper = mapper;
        _problemService = problemService;
    }

    [HttpPost]
    public async Task<IActionResult> GetProblemListAsync([FromBody] ProblemListRequestDto dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new ProblemListRequestModel()
        {
            UserId = userId,
            SubjectId = dto.SubjectId,
            SearchTerm = dto.SearchTerm,
            SearchType = dto.SearchType,
            Difficulty = dto.Difficulty,
            IsPublic = dto.IsPublic,
            IsPublished = dto.IsPublished,
            OnlyMyProblems = dto.OnlyMyProblems,
            PageNumber = dto.PageNumber,
            PageSize = dto.PageSize,
            TopicId = dto.TopicId,
            Type = dto.Type

        };

        OperationResult<PaginatedListResponseModel<ProblemPreviewModel>> result = await _problemService.GetProblemListAsync(requestModel);

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

    [HttpGet("{id : long}")]
    public async Task<IActionResult> GetByIdAsync(long problemId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new ProblemDetailsRequestModel()
        {
            UserId = userId,
            ProblemId = problemId   
        };

        OperationResult<ProblemDetailsModel> result = await _problemService.GetProblemDetailsAsync(requestModel);

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
