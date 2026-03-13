using CTHelper.Application.Common.Constants;
using CTHelper.Application.Common.Helpers;
using CTHelper.Application.Exceptions;
using CTHelper.Application.Models;
using CTHelper.Application.Models.User;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Application.Specification.UserSpecifications;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<LoginResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(
        IUnitOfWork unitOfWork,
        ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
    }

    public async Task<OperationResult<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var userPassword = await _unitOfWork.Users.GetAsync<UserPasswordModel>(
            new UserPasswordByEmailAsNoTrackingSpecification(request.Email),
            cancellationToken);

        if (userPassword == null)
        {
            return new OperationResult<LoginResponse>
            {
                ErrorMessage = "User doesn't exist",
                ErrorCode = ErrorCodeConstants.UserDoesntExist,
                HttpStatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        var passwordHash = HashHelper.Get128Hash(request.Password);
        if (userPassword.PasswordHash != passwordHash)
        {
            return new OperationResult<LoginResponse>
            {
                ErrorMessage = "Wrong password",
                ErrorCode = ErrorCodeConstants.WrongPassword,
                HttpStatusCode = System.Net.HttpStatusCode.Unauthorized
            };
        }

        var userToken = await _unitOfWork.Users.GetAsync<UserTokenModel>(
            new UserTokenByEmailAsNoTrackingSpecification(request.Email),
            cancellationToken);

        var sessionJti = Guid.NewGuid();
        
        var accessToken = _tokenService.GenerateAccessToken(userToken!, sessionJti);
        var refreshTokenString = _tokenService.GenerateRefreshToken();

        var session = new UserSession
        {
            UserId = userPassword.Id,
            Jti = sessionJti,
            ClientType = request.ClientType,
            IpAddress = request.IpAddress,
            DeviceInfo = request.DeviceInfo,
            LastActivityAt = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow,
            LastUpdateAt = DateTimeOffset.UtcNow
        };

        await _unitOfWork.UserSessions.AddAsync(session, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var refreshToken = new RefreshToken
        {
            SessionId = session.Id,
            TokenHash = HashHelper.Get128Hash(refreshTokenString),
            DeviceId = request.DeviceId ?? string.Empty,
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(_tokenService.GetRefreshTokenExpirationDays()),
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _unitOfWork.RefreshTokens.AddAsync(refreshToken, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new OperationResult<LoginResponse>
        {
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            Payload = new LoginResponse
            {
                UserId = userToken!.Id,
                Username = userToken.Username,
                Email = userToken.Email,
                Role = userToken.Role,
                AccessToken = accessToken,
                RefreshToken = refreshTokenString,
                SessionJti = sessionJti
            }
        };
    }
}
