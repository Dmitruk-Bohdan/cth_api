using CTHelper.Application.Models;
using CTHelper.Application.Services.Interfaces;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<LoginResponse>>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<OperationResult<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await _authService.LoginAsync(request, cancellationToken);
    }
}