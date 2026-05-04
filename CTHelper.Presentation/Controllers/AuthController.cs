using CTHelper.Application.Models.Session;
using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Application.UseCases.Identity.Query;
using CTHelper.Domain.Common.Extensions;
using CTHelper.Infrastructure.Settings;
using CTHelper.Presentation.Common.Constants;
using CTHelper.Presentation.Dtos;
using CTHelper.Presentation.Dtos.AuthDtos;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly JwtSettings _jwtSettings;
    private readonly IMapper _mapper;
    public AuthController(
        IMediator mediator,
        IMapper mapper,
        IOptions<JwtSettings> jwtSettings)
    {
        _mediator = mediator;
        _mapper = mapper;
        _jwtSettings = jwtSettings.Value;
    }

    [HttpPost]
    [Route("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto request)
    {
        var createUserCommand = _mapper.Map<RegisterUserCommand>(request);
        var createdUser = await _mediator.Send(createUserCommand);

        var requestEmailVerificationCommand = _mapper.Map<RequestEmailVerificationCommand>(createdUser);
        await _mediator.Send(requestEmailVerificationCommand);

        return Created();
    }

#if DEBUG
    [HttpPost("login_as_teacher")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginAsTeacher()
    {
        return await Login(new LoginRequestDto() { Email = "teacher1@teacher1.com", Password = "111111" });
    }

    [HttpPost("login_as_student")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginAsStudent()
    {
        return await Login(new LoginRequestDto() { Email = "student1@student1.com", Password = "111111" });
    }

    [HttpPost("login_as_admin")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginAsAdmin()
    {
        return await Login(new LoginRequestDto() { Email = "admin1@admin1.com", Password = "111111" });
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var loginCommand = _mapper.Map<LoginCommand>(request);
        var result = await _mediator.Send(loginCommand);

        if (result.ErrorCode == null)
        {

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,      
                Secure = true,        
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                Path = "/auth"
            };

            Response.Cookies.Append(
                CookieConstants.RefreshToken,
                result.Payload!.RefreshToken,
                cookieOptions);
            
            var responseDto = _mapper.Map<LoginResponseDto>(result.Payload);
            return Ok(responseDto);
        }
        else
        {
            var errorDto = _mapper.Map<ErrorResponseDto>(result);
            return StatusCode(
                result.HttpStatusCode.ToInt(),
                errorDto);
        }
    }

#endif

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout()
    {
        var sessionJtiClaim = User.FindFirstValue(JwtRegisteredClaimNames.Jti);

        if (sessionJtiClaim == null || !Guid.TryParse(sessionJtiClaim, out var sessionJti))
        {
            return Unauthorized();
        }

        var logoutCommand = new LogoutCommand(sessionJti);
        await _mediator.Send(logoutCommand);

        return NoContent();
    }

    [HttpPost("logout-all")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> LogoutAll()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var logoutCommand = new LogoutFromAllDevicesCommand(userId);
        await _mediator.Send(logoutCommand);

        return NoContent();
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken()
    {
        var sessionsJwt = User.FindFirstValue(JwtRegisteredClaimNames.Jti);
        if(string.IsNullOrWhiteSpace(sessionsJwt))
        {
            return Unauthorized();
        }

        var refreshToken = Request.Cookies[CookieConstants.RefreshToken];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized();
        }

        var refreshTokenCommand = new RefreshTokenCommand(
            refreshToken,
            Guid.Parse(sessionsJwt)
        );

        var result = await _mediator.Send(refreshTokenCommand);

        if (result.ErrorCode == null)
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

    [HttpPost("request-password-reset")]
    [EnableRateLimiting(PoliciesNamesConstants.OtpDeliveryPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetRequestDto request)
    {
        var command = _mapper.Map<RequestPasswordResetCommand>(request);
        var result = await _mediator.Send(command);

        if (result.ErrorCode == null)
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

    [HttpPost("confirm-password-reset")]
    [EnableRateLimiting(PoliciesNamesConstants.OtpDeliveryPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmPasswordReset([FromBody] ConfirmPasswordResetRequestDto request)
    {
        var command = _mapper.Map<ConfirmPasswordResetCommand>(request);
        var result = await _mediator.Send(command);

        if (result.ErrorCode == null)
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

    [HttpPost("request-email-verification")]
    [EnableRateLimiting(PoliciesNamesConstants.ResendEmailPolicy)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> RequestEmailVerification([FromBody] RequestEmailVerificationRequestDto request)
    {
        var requestEmailVerificationCommand = _mapper.Map<RequestEmailVerificationCommand>(request);
        var result = await _mediator.Send(requestEmailVerificationCommand);

        if (result.ErrorCode == null)
        {
            return Created();
        }
        else
        {
            var errorDto = _mapper.Map<ErrorResponseDto>(result);
            return StatusCode(
                result.HttpStatusCode.ToInt(),
                errorDto);
        }
    }

    [HttpPost("confirm-email-verification")]
    [EnableRateLimiting(PoliciesNamesConstants.ResendEmailPolicy)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> ConfirmEmailVerification([FromBody] ConfirmEmailVerificationDto request)
    {
        var confirmEmailCommand = _mapper.Map<ConfirmEmailVerificationCommand>(request);
        var result = await _mediator.Send(confirmEmailCommand);
        
        if(result.ErrorCode == null)
        {
            return Created();

        }
        else
        {
            var errorDto = _mapper.Map<ErrorResponseDto>(result);
            return StatusCode(
                result.HttpStatusCode.ToInt(),
                errorDto);
        }
    }

    [HttpGet("me/sessions")]
    [Authorize]
    [ProducesResponseType(typeof(List<UserSessionListResponseModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMySessions()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var query = new GetMySessionListQuery(userId);
        var sessions = await _mediator.Send(query);

        return Ok(sessions);
    }
}
