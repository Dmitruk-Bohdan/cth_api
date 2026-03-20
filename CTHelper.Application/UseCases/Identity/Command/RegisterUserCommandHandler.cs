using CTHelper.Application.Services.Interfaces;
using MediatR;
using User = CTHelper.Domain.Entities.User;

namespace CTHelper.Application.UseCases.Identity.Command
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, User>
    {
        private readonly IAuthService _authService;

        public RegisterUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<User> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RegisterUserAsync(request, cancellationToken);
        }
    }
}