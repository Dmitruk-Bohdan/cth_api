using CTHelper.Application.Models;
using CTHelper.Application.Services.Interfaces;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public class ConfirmEmailVerificationCommandHandler : IRequestHandler<ConfirmEmailVerificationCommand, OperationResult>
{
    private readonly IAuthService _authService;

    public ConfirmEmailVerificationCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<OperationResult> Handle(ConfirmEmailVerificationCommand request, CancellationToken cancellationToken)
    {
        return await _authService.ConfirmEmailAsync(request.Email, request.TokenAsString, cancellationToken);
    }
}