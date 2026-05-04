using CTHelper.Application.Models;
using CTHelper.Application.Models.TestAttemptModels;
using CTHelper.Application.Models.TestModels;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Presentation.Common.Constants;
using CTHelper.Presentation.Dtos;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("attempts")]
public class TestAttemptsController : BaseController
{
    private readonly ITestAttemptService _attemptService;

    public TestAttemptsController(ITestAttemptService attemptService, IMapper mapper) : base(mapper)
    {
        _attemptService = attemptService;
    }

    [HttpGet("me/list")]
    [Authorize(Policy = PoliciesNamesConstants.StudentOnlyPolicy)]
    [ProducesResponseType(typeof(PaginatedListResponseModel<TestAttemptListItemModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyAttemptListAsync([FromQuery] string? searchTerm, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (!TryGetUserId(out long userId))
        {
            return Unauthorized();
        }

        var requestModel = new MyTestAttemptListRequestModel
        {
            UserId = userId,
            TestNameFragment = searchTerm,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        OperationResult<PaginatedListResponseModel<TestAttemptListItemModel>> result = await _attemptService.GetMyAttemptList(requestModel);

        return HandleOperationResult(result);
    }

    [HttpGet("me/{attemptId:long}")]
    [Authorize(Policy = PoliciesNamesConstants.StudentOnlyPolicy)]
    [ProducesResponseType(typeof(TestAttemptDetails), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyAttemptAsync([FromRoute] long attemptId)
    {
        if (!TryGetUserId(out long userId))
        {
            return Unauthorized();
        }

        var requestModel = new MyTestAttemptRequestModel
        {
            UserId = userId,
            AttemptId = attemptId
        };

        OperationResult<TestAttemptDetails> result = await _attemptService.GetMyAttempt(requestModel);

        return HandleOperationResult(result);
    }

    [HttpGet("teacher/student/{studentId:long}/list")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(typeof(PaginatedListResponseModel<TestAttemptListItemModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentAttemptListAsync([FromQuery] string? searchTerm, [FromRoute] long studentId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (!TryGetUserId(out long userId))
        {
            return Unauthorized();
        }

        var requestModel = new StudentTestAttemptListRequestModel
        {
            UserId = userId,
            StudentId = studentId,
            TestNameFragment = searchTerm,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        OperationResult<PaginatedListResponseModel<TestAttemptListItemModel>> result = await _attemptService.GetStudentAttemptList(requestModel);

        return HandleOperationResult(result);
    }

    [HttpGet("teacher/student/{studentId:long}/attempt/{attemptId:long}")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(typeof(TestAttemptDetails), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentAttemptAsync([FromRoute] long studentId, [FromRoute] long attemptId)
    {
        if (!TryGetUserId(out long userId))
        {
            return Unauthorized();
        }

        var requestModel = new StudentTestAttemptRequestModel
        {
            UserId = userId,
            StudentId = studentId,
            AttemptId = attemptId
        };

        OperationResult<TestAttemptDetails> result = await _attemptService.GetStudentAttempt(requestModel);

        return HandleOperationResult(result);
    }

    [HttpPost("start/{testId:long}")]
    [Authorize]
    [ProducesResponseType(typeof(TestPassingResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> StartAttemptAsync([FromRoute] long testId)
    {
        if (!TryGetUserId(out long userId))
        {
            return Unauthorized();
        }

        var requestModel = new StartTestAttemptRequestModel
        {
            UserId = userId,
            TestId = testId
        };

        OperationResult<TestPassingResponseModel> result = await _attemptService.StartTestAttempt(requestModel);

        return HandleOperationResult(result);
    }

    [HttpPatch("{attemptId:long}/pause")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> PauseAttemptAsync([FromRoute] long attemptId, [FromBody] IEnumerable<UserAnswerDto>? userAnswers)
    {
        if (!TryGetUserId(out long userId))
        {
            return Unauthorized();
        }

        var requestModel = new PauseTestAttemptRequestModel
        {
            UserId = userId,
            AttemptId = attemptId,
            UserAnswers = userAnswers
        };

        OperationResult result = await _attemptService.PauseTestAttempt(requestModel);

        return HandleOperationResult(result);
    }

    [HttpPatch("{attemptId:long}/resume")]
    [Authorize]
    [ProducesResponseType(typeof(TestPassingResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> ResumeAsync([FromRoute] long attemptId)
    {
        if (!TryGetUserId(out long userId))
        {
            return Unauthorized();
        }

        var requestModel = new ResumeTestAttemptRequestModel
        {
            UserId = userId,
            AttemptId = attemptId,
        };

        OperationResult<TestPassingResponseModel> result = await _attemptService.ResumeTestAttempt(requestModel);

        return HandleOperationResult(result);
    }

    [HttpPatch("{attemptId:long}/complete")]
    [Authorize]
    [ProducesResponseType(typeof(CompleteTestAttemptResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> CompleteAsync([FromRoute] long attemptId, [FromBody] IEnumerable<UserAnswerDto>? userAnswers)
    {
        if (!TryGetUserId(out long userId))
        {
            return Unauthorized();
        }

        var requestModel = new CompleteTestAttemptRequestModel
        {
            UserId = userId,
            AttemptId = attemptId,
            UserAnswers = userAnswers
        };

        OperationResult<CompleteTestAttemptResponseModel> result = await _attemptService.CompleteTestAttempt(requestModel);

        return HandleOperationResult(result);
    }

    [HttpPatch("{attemptId:long}/cancel")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CancelAsync([FromRoute] long attemptId)
    {
        if (!TryGetUserId(out long userId))
        {
            return Unauthorized();
        }

        var requestModel = new CancelTestAttemptRequestModel
        {
            UserId = userId,
            AttemptId = attemptId,
        };

        OperationResult result = await _attemptService.CancelTestAttempt(requestModel);

        return HandleOperationResult(result);
    }
}