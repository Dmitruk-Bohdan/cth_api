using CTHelper.Application.Models;
using CTHelper.Application.Models.TestAttemptModels;
using CTHelper.Application.Models.TestModels;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Common.Extensions;
using CTHelper.Domain.Entities;
using CTHelper.Infrastructure.Services.Implementations;
using CTHelper.Presentation.Common.Constants;
using CTHelper.Presentation.Dtos;
using CTHelper.Presentation.Dtos.TestDtos;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("tests")]
public class TestController : BaseController
{
    private readonly ITestService _testService;
    private readonly ITestAttemptService _testAttemptService;
    public TestController(IMapper mapper, ITestService testService, ITestAttemptService testAttemptService) : base(mapper)
    {
        _testService = testService;
        _testAttemptService = testAttemptService;
    }

    [HttpPost("teacher/list")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(typeof(PaginatedListResponseModel<TestListItemModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTestListTeacher([FromBody] TeacherTestListRequestDto request)
    {
        if (!TryGetUserId(out long userId))
        {
            return Unauthorized();
        }

        var requestModel = new TeacherTestListRequestModel()
        {
            NameFragment = request.NameFragment,
            AuthorNameFragment = request.AuthorNameFragment,
            AvgDifficult = request.AvgDifficult,
            IsTraning = request.IsTraning,
            Type = request.Type,
            MaxTaskCount = request.MaxTaskCount,
            MinTaskCount = request.MinTaskCount,

            PageSize = request.PageSize,
            PageNumber = request.PageNumber,

            OnlyMyTests = request.OnlyMyTests,
            UserId = userId  
        };

        OperationResult<PaginatedListResponseModel<TestListItemModel>> result = await _testService.GetTestList(requestModel);

        return HandleOperationResult(result);
    }

    [HttpPost("student/list")]
    [Authorize(Policy = PoliciesNamesConstants.StudentOnlyPolicy)]
    [ProducesResponseType(typeof(PaginatedListResponseModel<TestListItemModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTestListStudentAsync([FromBody] StudentTestListRequestDto request)
    {
        if (!TryGetUserId(out long userId))
        {
            return Unauthorized();
        }

        var requestModel = new StudentTestListRequestModel()
        {
            NameFragment = request.NameFragment,
            AuthorNameFragment = request.AuthorNameFragment,
            AvgDifficult = request.AvgDifficult,
            IsTraning = request.IsTraning,
            Type = request.Type,
            MaxTaskCount = request.MaxTaskCount,
            MinTaskCount = request.MinTaskCount,

            PageSize = request.PageSize,
            PageNumber = request.PageNumber,

            AssignedToMe = request.AssignedToMe,
            UserId = userId
        };

        OperationResult<PaginatedListResponseModel<TestListItemModel>> result = await _testService.GetTestList(requestModel);

        return HandleOperationResult(result);
    }

    [HttpGet("{testId:long}/details")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(typeof(TestDetailsModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTestDetailsAsync([FromRoute] long testId)
    {
        if (!TryGetUserId(out long userId))
        {
            return Unauthorized();
        }

        var requestModel = new TestDetailsRequestModel()
        {
            TestId = testId,
            UserId = userId
        };
        
        OperationResult<TestDetailsModel> result = await _testService.GetTestDetails(requestModel);

        return HandleOperationResult(result);
    }


    [HttpGet("{testId:long}/preview")]
    [Authorize]
    [ProducesResponseType(typeof(TestPreviewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTestPreviewAsync([FromRoute] long testId)
    {
        if (!TryGetUserId(out long userId))
        {
            return Unauthorized();
        }

        var requestModel = new TestPreviewRequestModel()
        {
            TestId = testId,
            UserId = userId
        };

        OperationResult<TestPreviewModel> result = await _testService.GetTestPreview(requestModel);

        return HandleOperationResult(result);
    }

    [HttpPost]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateTestRequestDto request)
    {
        if (!TryGetUserId(out long userId))
        {
            return Unauthorized();
        }

        var requestModel = new CreateTestRequestModel
        {
            Title = request.Title,
            SubjectId = request.SubjectId,
            AuthorId = userId,
            IsTraning = request.IsTraning,
            IsPublished = request.IsPublished,
            IsPublic = request.IsPublic,
            Duration = request.Duration,
            AttemptsCount = request.AttemptsCount,
            TestProblemList = request.TestProblemList.Select(x => new TestProblemCodeModel
            {
                ProblemId = x.ProblemId,
                Code = x.Code
            })
        };

        OperationResult result = await _testService.CreateTest(requestModel);
        
        return HandleOperationResult(result);
    }

    [HttpPost("init-mixed")]
    [Authorize]
    [ProducesResponseType(typeof(Test), StatusCodes.Status200OK)]
    public async Task<IActionResult> InitMixedTestAsync([FromBody] CreateMixedTestRequestDto request)
    {
        if (!TryGetUserId(out long userId))
        {
            return Unauthorized();
        }

        var requestModel = new CreateMixedTestRequestModel
        {
            AuthorId = userId,
            SubjectId = request.SubjectId,
            AverageDifficult = request.AverageDifficult,
            TopicItems = request.TopicItems.Select(x => new MixedTestTopicModel
            {
                TopicId = x.TopicId,
                ProblemCount = x.ProblemCount
            })
        };

        OperationResult<Test> result = await _testService.CreateMixedTest(requestModel);


        if (result.IsSuccess)
        {
            OperationResult startTesResult = await _testAttemptService.StartTestAttempt(new StartTestAttemptRequestModel() { UserId = userId, TestId = result.Payload!.Id});

            return HandleOperationResult(startTesResult);
        }
        else
        {
            var errorDto = _mapper.Map<ErrorResponseDto>(result);
            return StatusCode(
                result.HttpStatusCode.ToInt(),
                errorDto);
        }
    }

    [HttpPut("{testId:long}")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync(long id, [FromBody] UpdateTestRequestDto request)
    {
        if (!TryGetUserId(out long userId))
        {
            return Unauthorized();
        }

        var requestModel = new UpdateTestRequestModel
        {
            UserId = userId,
            TestId = id,
            Title = request.Title,
            IsTraning = request.IsTraning,
            IsPublished = request.IsPublished,
            IsPublic = request.IsPublic,
            Duration = request.Duration,
            AttemptsCount = request.AttemptsCount,
            TestProblemIdList = request.TestProblemList.Select(x => new TestProblemCodeModel
            {
                ProblemId = x.ProblemId,
                Code = x.Code
            })
        };

        OperationResult result = await _testService.UpdateTest(requestModel);
        return HandleOperationResult(result);
    }

    [HttpDelete("{testId:long}")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveAsync(long testId)
    {
        if (!TryGetUserId(out long userId))
        {
            return Unauthorized();
        }

        var requestModel = new RemoveTestRequestModel()
        {
            UserId = userId,
            TestId = testId
        };

        OperationResult result = await _testService.RemoveTest(requestModel);

        return HandleOperationResult(result);
    }
}
