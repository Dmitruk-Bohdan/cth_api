using CTHelper.Application.Common.Constants;
using CTHelper.Application.Common.Helpers;
using CTHelper.Application.Models;
using CTHelper.Application.Models.User;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Application.Specification.RefreshToken;
using CTHelper.Application.Specification.UserSpecifications;
using CTHelper.Application.Specification.UserSession;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, OperationResult<LoginResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(
        IUnitOfWork unitOfWork,
        ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
    }

    public async Task<OperationResult<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenHash = HashHelper.Get128Hash(request.RefreshToken);

        var validRefreshTokens = await _unitOfWork.RefreshTokens.GetListAsync(
            new ActiveRefreshTokenByHashSpecification(tokenHash),
            cancellationToken);

        var validRefreshToken = validRefreshTokens.FirstOrDefault();

        if (validRefreshToken == null)
        {
            return new OperationResult<LoginResponse>
            {
                ErrorMessage = "Invalid refresh token",
                ErrorCode = ErrorCodeConstants.InvalidRefreshToken,
                HttpStatusCode = System.Net.HttpStatusCode.Unauthorized
            };
        }

        if (validRefreshToken.ExpiresAt < DateTimeOffset.UtcNow)
        {
            return new OperationResult<LoginResponse>
            {
                ErrorMessage = "Refresh token has expired",
                ErrorCode = ErrorCodeConstants.RefreshTokenExpired,
                HttpStatusCode = System.Net.HttpStatusCode.Unauthorized
            };
        }

        var sessions = await _unitOfWork.UserSessions.GetListAsync(
            new UserSessionByIdSpecification(validRefreshToken.SessionId),
            cancellationToken);

        var validSession = sessions.FirstOrDefault();

        if (validSession == null)
        {
            return new OperationResult<LoginResponse>
            {
                ErrorMessage = "Session not found",
                ErrorCode = ErrorCodeConstants.InvalidRefreshToken,
                HttpStatusCode = System.Net.HttpStatusCode.Unauthorized
            };
        }

        var userToken = await _unitOfWork.Users.GetAsync(
            new UserTokenByIdAsNoTrackingSpecification(validSession.UserId),
            cancellationToken);

        if (userToken == null)
        {
            return new OperationResult<LoginResponse>
            {
                ErrorMessage = "User not found",
                ErrorCode = ErrorCodeConstants.UserDoesntExist,
                HttpStatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        var newSessionJti = Guid.NewGuid();

        validRefreshToken.RevokedAt = DateTimeOffset.UtcNow;
        await _unitOfWork.RefreshTokens.UpdateAsync(validRefreshToken, cancellationToken);

        var newAccessToken = _tokenService.GenerateAccessToken(userToken, newSessionJti);
        var newRefreshTokenString = _tokenService.GenerateRefreshToken();

        validSession.Jti = newSessionJti;
        validSession.LastActivityAt = DateTimeOffset.UtcNow;
        validSession.LastUpdateAt = DateTimeOffset.UtcNow;
        await _unitOfWork.UserSessions.UpdateAsync(validSession, cancellationToken);

        var newRefreshToken = new RefreshToken
        {
            SessionId = validSession.Id,
            TokenHash = HashHelper.Get128Hash(newRefreshTokenString),
            DeviceId = request.DeviceId ?? validRefreshToken.DeviceId,
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(_tokenService.GetRefreshTokenExpirationDays()),
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _unitOfWork.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new OperationResult<LoginResponse>
        {
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            Payload = new LoginResponse
            {
                UserId = userToken.Id,
                Username = userToken.Username,
                Email = userToken.Email,
                Role = userToken.Role,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshTokenString,
                SessionJti = newSessionJti
            }
        };
    }
}
