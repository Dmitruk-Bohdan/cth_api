using CTHelper.Application.Models;
using CTHelper.Application.Services.Interfaces;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public class LogoutFromAllDeviCommandHandler : IRequestHandler<LogoutFromAllDeviCommand, OperationResult>
{
    private readonly IAuthService _authService;

    public LogoutFromAllDeviCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<OperationResult> Handle(LogoutFromAllDeviCommand request, CancellationToken cancellationToken)
    {
        return await _authService.LogoutFromAllDevicesAsync(request.UserId, cancellationToken);
    }
}