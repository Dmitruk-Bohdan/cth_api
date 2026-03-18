using CTHelper.Application.Models;
using CTHelper.Application.Services.Interfaces;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, OperationResult<RefreshTokenResponse>>
    {
        private readonly IAuthService _authService;

        public RefreshTokenCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<OperationResult<RefreshTokenResponse>> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            return _authService.RefreshAccessTokenAsync(
                request.SessionJti,
                request.RefreshToken,
                cancellationToken
            );
        }
    }
}