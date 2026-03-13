using CTHelper.Application.Models;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public record RefreshTokenCommand(
    string RefreshToken,
    long? UserId,
    string? DeviceId) : IRequest<OperationResult<LoginResponse>>;
