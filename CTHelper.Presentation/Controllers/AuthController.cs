using CTHelper.Application.Models.Dtos;
using CTHelper.Application.Models.Dtos.AuthDtos;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private IMediator _mediator;
    private IMapper _mapper;
    public AuthController(
        IMediator mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto dto)
    {
        var createUserCommand = _mapper.Map<CreateUserCommand>(dto);
        var createdUserId = await _mediator.Send(createUserCommand);

        return CreatedAtAction(
            nameof(UsersController.GetById),
            nameof(UsersController),
            new IdDto(createdUserId));
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        throw new NotImplementedException();
    }

    [HttpPost("logout-all")]
    public IActionResult LogoutAll()
    {
        throw new NotImplementedException();
    }

    [HttpPost("refresh-token")]
    public IActionResult RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpPost("request-password-reset")]
    public IActionResult RequestPasswordReset([FromBody] RequestPasswordResetRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpPost("confirm-password-reset")]
    public IActionResult ConfirmPasswordReset([FromBody] ConfirmPasswordResetRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpPost("request-email-verification")]
    public IActionResult RequestEmailVerification([FromBody] RequestEmailVerificationRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpPost("confirm-email-verification")]
    public IActionResult ConfirmEmailVerification([FromBody] ConfirmEmailVerificationRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpGet("me/sessions")]
    public IActionResult GetMySessions()
    {
        throw new NotImplementedException();
    }
}
