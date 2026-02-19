using Microsoft.AspNetCore.Mvc;
using CTHelper.Application.Models.Dtos.AuthDtos;

namespace CTHelper.Presentation.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
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
