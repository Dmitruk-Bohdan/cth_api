using CTHelper.Application.Models;
using CTHelper.Domain.Common.Enums;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public record LoginCommand(
    string Email,
    string Password,
    ClientType ClientType,
    string? IpAddress,
    string? DeviceInfo,
    string? DeviceId) : IRequest<OperationResult<LoginResponse>>;

public class LoginResponse
{
    public long UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public Guid SessionJti { get; set; }
}
