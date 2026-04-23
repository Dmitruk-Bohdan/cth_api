using CTHelper.Application.Models;
using CTHelper.Application.Models.Group;
using CTHelper.Application.Models.Statistics;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Common.Extensions;
using CTHelper.Infrastructure.Services.Implementations;
using CTHelper.Presentation.Common.Constants;
using CTHelper.Presentation.Dtos;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("statistics")]
public class StatisticsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService, IMapper mapper)
    {
        _statisticsService = statisticsService;
        _mapper = mapper;
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(StudentStatisticsModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyStatisticsAsync([FromQuery] long subjectId, [FromQuery] DateTimeOffset? dateFrom, [FromQuery] DateTimeOffset? dateTo)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new MyStatisticsBySubjectRequestModel
        {
            UserId = userId,
            SubjectId = subjectId,
            DateFrom = dateFrom,
            DateTo = dateTo
        };

        OperationResult<StudentStatisticsModel> result = await _statisticsService.GetMyStatisticsBySubject(requestModel);

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

    [HttpGet("students/{id}")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(typeof(StudentStatisticsModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentStatisticsAsync([FromRoute] long studentId, [FromQuery] long subjectId, [FromQuery] DateTimeOffset? dateFrom, [FromQuery] DateTimeOffset? dateTo)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new StudentStatisticsBySubjectRequestModel
        {
            UserId = userId,
            StudentId = studentId,
            SubjectId = subjectId,
            DateFrom = dateFrom,
            DateTo = dateTo
        };

        OperationResult<StudentStatisticsModel> result = await _statisticsService.GetStudentStatisticsBySubject(requestModel);

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

    [HttpGet("groups/{id}")]
    [Authorize(Policy = PoliciesNamesConstants.TeacherOnlyPolicy)]
    [ProducesResponseType(typeof(GroupStatisticsModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGroupStatisticsAsync([FromRoute] long groupId, [FromQuery] long subjectId, [FromQuery] DateTimeOffset? dateFrom, [FromQuery] DateTimeOffset? dateTo)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var requestModel = new GroupStatisticsBySubjectRequestModel()
        {
            UserId = userId,
            GroupId = groupId,
            SubjectId = subjectId,
            DateFrom = dateFrom,
            DateTo = dateTo
        };

        OperationResult<GroupStatisticsModel> result = await _statisticsService.GetGroupStatisticsBySubject(requestModel);

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
}
