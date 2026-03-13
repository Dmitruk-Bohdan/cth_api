using CTHelper.Application.Models;
using CTHelper.Application.Services.Interfaces;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public class ConfirmPasswordResetCommandHandler : IRequestHandler<ConfirmPasswordResetCommand, OperationResult>
{
    private readonly IAuthService _authService;

    public ConfirmPasswordResetCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<OperationResult> Handle(ConfirmPasswordResetCommand request, CancellationToken cancellationToken)
    {
        return await _authService.ConfirmPasswordResetAsync(request.Email, request.Token, request.NewPassword, cancellationToken);
    }
}