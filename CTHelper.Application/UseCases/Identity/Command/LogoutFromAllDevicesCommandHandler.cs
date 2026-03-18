using CTHelper.Application.Models;
using CTHelper.Application.Services.Interfaces;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public class LogoutFromAllDevicesCommandHandler : IRequestHandler<LogoutFromAllDevicesCommand, OperationResult>
{
    private readonly IAuthService _authService;

    public LogoutFromAllDevicesCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<OperationResult> Handle(LogoutFromAllDevicesCommand request, CancellationToken cancellationToken)
    {
        return await _authService.LogoutFromAllDevicesAsync(request.UserId, cancellationToken);
    }
}