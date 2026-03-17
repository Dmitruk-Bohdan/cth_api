using CTHelper.Application.Models;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Application.UseCases.Identity.Command.ResponseModels;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<LoginResponseModel>>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<OperationResult<LoginResponseModel>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await _authService.LoginAsync(request, cancellationToken);
    }
}