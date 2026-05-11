using CTHelper.Application.Models;
using CTHelper.Application.Models.Favourite;
using CTHelper.Application.Models.Problem;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Common.Extensions;
using CTHelper.Domain.Entities;
using CTHelper.Presentation.Common.Constants;
using CTHelper.Presentation.Dtos;
using CTHelper.Presentation.Dtos.GroupDtos;
using CTHelper.Presentation.Dtos.ProblemDtos;
using MailKit.Search;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Claims;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("problems")]
public class ProblemsController : BaseController
{
    private readonly IProblemService _problemService;
    public ProblemsController(IMapper mapper, IProblemService problemService) : base(mapper)
    {
        _problemService = problemService;
    }

    [HttpPost("list")]
    [Authorize]
    [ProducesResponseType(typeof(PaginatedListResponseModel<ProblemListItemModel>), StatusCodes.Status200OK)]
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

        OperationResult<PaginatedListResponseModel<ProblemListItemModel>> result = await _problemService.GetProblemListAsync(requestModel);

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

    [HttpGet("{problemId:long}")]
    [Authorize]
    [ProducesResponseType(typeof(ProblemDetailsModel), StatusCodes.Status200OK)]
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
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateProblemRequestDto request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new CreateProblemRequestModel
        {
            AuthorId = userId,  
            Type = request.Type,
            Difficulty = request.Difficulty,
            Statement = request.Statement,
            correctAnswer = request.correctAnswer,
            Explanation = request.Explanation,
            TopicId = request.TopicId,
            IsPublished = request.IsPublished,
            IsPublic = request.IsPublic
        };

        OperationResult result = await _problemService.CreateProblem(requestModel);

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
        throw new NotImplementedException();
    }

    [HttpPut("{problemId:long}")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync([FromRoute] long problemId, [FromBody] UpdateProblemRequestDto request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new UpdateProblemRequestModel
        {
            ProblemId = problemId,
            AuthorId = userId,
            Difficulty = request.Difficulty,
            Statement = request.Statement,
            correctAnswer = request.correctAnswer,
            Explanation = request.Explanation,
            TopicId = request.TopicId,
            IsPublished = request.IsPublished,
            IsPublic = request.IsPublic
        };

        OperationResult result = await _problemService.UpdateProblem(requestModel);

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

    [HttpDelete("{id}")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new DeleteProblemRequestModel
        {
            UserId = userId,
            ProblemId = id,
        };

        OperationResult result = await _problemService.DeleteProblem(requestModel);

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
