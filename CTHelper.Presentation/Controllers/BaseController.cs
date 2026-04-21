using CTHelper.Application.Models;
using CTHelper.Domain.Common.Extensions;
using CTHelper.Presentation.Dtos;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace CTHelper.Presentation.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IMapper _mapper;

        protected BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected bool TryGetUserId(out long userId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return long.TryParse(userIdClaim, out userId);
        }

        protected IActionResult HandleOperationResult(OperationResult result)
        {
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

        protected IActionResult HandleOperationResult<T>(OperationResult<T> result)
        {
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
}
