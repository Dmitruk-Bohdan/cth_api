using CTHelper.Application.Services.Interfaces;
using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Domain.Entities;
using MediatR;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, User>
{
    private readonly IAuthService _authService;

    public RegisterUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public Task<User> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        return _authService.RegisterUserAsync(request, cancellationToken);
    }
}