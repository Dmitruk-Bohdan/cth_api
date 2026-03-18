using CTHelper.Application.Common.Constants;
using CTHelper.Application.Common.Helpers;
using CTHelper.Application.Models;
using CTHelper.Application.Models.Session;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Application.Specification.EmailVerificationTokenSpecifications;
using CTHelper.Application.Specification.PasswordResetToken;
using CTHelper.Application.Specification.RefreshToken;
using CTHelper.Application.Specification.UserSession;
using CTHelper.Application.Specification.UserSpecifications;
using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Application.UseCases.Identity.Command.ResponseModels;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Common.Extensions;
using CTHelper.Domain.Entities;
using CTHelper.Infrastructure.Settings;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Options;
using System.Net;

namespace CTHelper.Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityTokenService _tokenService;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly IShortTokenService _shortTokenService;
    private readonly IMapper _mapper;
    private readonly TokenSettings _tokenSettings;

    public AuthService(
        IUnitOfWork unitOfWork,
        IIdentityTokenService tokenService,
        IPasswordHashingService passwordHashingService,
        IShortTokenService shortTokenService,
        IMapper mapper,
        IOptions<TokenSettings> tokenSettings)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _passwordHashingService = passwordHashingService;
        _shortTokenService = shortTokenService;
        _mapper = mapper;
        _tokenSettings = tokenSettings.Value;
    }

    public async Task<OperationResult<LoginResponseModel>> LoginAsync(LoginCommand request, CancellationToken cancellationToken)
    {
        var userToken = await _unitOfWork.Users.GetAsync(
            new UserTokenByEmailAsNoTrackingSpecification(request.Email),
            cancellationToken);

        if (userToken == null)
        {
            return OperationResultHelper.UserNotFoundTemplate<LoginResponseModel>(email: request.Email);
        }

        if (!_passwordHashingService.Verify(request.Password, userToken.PasswordHash))
        {
            return new OperationResult<LoginResponseModel>
            {
                ErrorMessage = "Wrong password",
                ErrorCode = ErrorCodeConstants.WrongPassword,
                HttpStatusCode = HttpStatusCode.Unauthorized
            };
        }

        if (request.DeviceId != null) {
            var activeSessionsOnCurrentDevice = await _unitOfWork.UserSessions.GetListAsync(
                new ActiveUserSessionsByUserIdAndDeviceIdIncludingRefreshTokenSpecification(userToken!.UserId, request.DeviceId));

            if (!activeSessionsOnCurrentDevice.IsNullOrEmpty())
            {
                foreach (var activeSession in activeSessionsOnCurrentDevice)
                {
                    activeSession.RevokedAt = DateTimeOffset.UtcNow;
                    activeSession.RefreshToken.RevokedAt = DateTimeOffset.UtcNow;
                }
            }
        }

        var sessionJti = Guid.NewGuid();
        var accessToken = _tokenService.GenerateAccessToken(userToken!, sessionJti);
        var refreshTokenString = _tokenService.GenerateRefreshToken();

        var session = new UserSession
        {
            UserId = userToken.UserId,
            Jti = sessionJti,
            ClientType = request.ClientType,
            IpAddress = request.IpAddress,
            DeviceInfo = request.DeviceInfo,
            LastActivityAt = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow,
            LastUpdateAt = DateTimeOffset.UtcNow
        };

        await _unitOfWork.UserSessions.AddAsync(session, cancellationToken);

        var refreshToken = new RefreshToken
        {
            Session = session,
            TokenHash = _tokenService.ComputeRefreshTokenHash(refreshTokenString),
            DeviceId = request.DeviceId,
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(_tokenService.GetRefreshTokenExpirationDays()),
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _unitOfWork.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new OperationResult<LoginResponseModel>
        {
            HttpStatusCode = HttpStatusCode.OK,
            Payload = new LoginResponseModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenString,
            }
        };
    }
    public async Task<OperationResult> ConfirmEmailAsync(string email, string tokenAsString, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetAsync(new UserByEmaiSpecification(email), cancellationToken);
        if (user == null)
        {
            return OperationResultHelper.UserNotFoundTemplate(email: email);
        }

        var token = await _unitOfWork.EmailVerificationTokens.GetAsync(
            new EmailConfirmationActiveTokenByUserEmailSpecification(email, user.Id),
            cancellationToken);

        if (token == null)
            return new OperationResult
            {
                ErrorMessage = "No active tokens were found for this user.",
                ErrorCode = ErrorCodeConstants.WrongEmailVerificationToken,
                HttpStatusCode = HttpStatusCode.BadRequest
            };

        if (token.ExpiresAt < DateTimeOffset.UtcNow)
        {
            var response = new OperationResult
            {
                ErrorMessage = "Token is expired, request a new one",
                ErrorCode = ErrorCodeConstants.EmailVerificationTokenIsExpired,
                HttpStatusCode = HttpStatusCode.BadRequest
            };

            await _unitOfWork.EmailVerificationTokens.DeleteAsync(token, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return response;
        }

        if (!_shortTokenService.Verify(tokenAsString, token.TokenHash))
        {
            token.AttemptsLeft--;

            var response = new OperationResult
            {
                ErrorMessage = $"Token doesn't match!, {token.AttemptsLeft} attempts left!",
                ErrorCode = ErrorCodeConstants.WrongEmailVerificationToken,
                HttpStatusCode = HttpStatusCode.BadRequest
            };

            if (token.AttemptsLeft <= 0)
                await _unitOfWork.EmailVerificationTokens.DeleteAsync(token, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return response;
        }

        user.IsEmailVerified = true;
        await _unitOfWork.EmailVerificationTokens.DeleteAsync(token, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new OperationResult { HttpStatusCode = HttpStatusCode.Created };
    }
    public async Task<OperationResult> ConfirmPasswordResetAsync(string email, string tokenAsString, string newPassword, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetAsync(new UserByEmaiSpecification(email), cancellationToken);

        if (user == null)
        {
            return OperationResultHelper.UserNotFoundTemplate(email: email);
        }

        var token = await _unitOfWork.PasswordResetTokens.GetAsync(
            new ActivePasswordResetTokenByUserEmailSpecification(user.Id),
            cancellationToken);

        if (token == null)
        {
            return new OperationResult
            {
                ErrorMessage = "No active tokens were found for this user.",
                ErrorCode = ErrorCodeConstants.NoActivePasswordResetTokenFound,
                HttpStatusCode = HttpStatusCode.BadRequest
            };
        }

        if (token.ExpiresAt < DateTimeOffset.UtcNow)
        {
            await _unitOfWork.PasswordResetTokens.DeleteAsync(token, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new OperationResult
            {
                ErrorMessage = "Token is expired, request a new one",
                ErrorCode = ErrorCodeConstants.PasswordResetTokenIsExpired,
                HttpStatusCode = HttpStatusCode.BadRequest
            };
        }

        var requestTokenHash = _shortTokenService.ComputeHash(tokenAsString);
        if (!_shortTokenService.Verify(tokenAsString, token.TokenHash))
        {
            token.AttemptsLeft--;

            if (token.AttemptsLeft <= 0)
            {
                await _unitOfWork.PasswordResetTokens.DeleteAsync(token, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new OperationResult
            {
                ErrorMessage = $"Token doesn't match!, {token.AttemptsLeft} attempts left!",
                ErrorCode = ErrorCodeConstants.WrongPasswordResetToken,
                HttpStatusCode = HttpStatusCode.BadRequest
            };
        }

        user.PasswordHash = _passwordHashingService.Hash(newPassword);

        await _unitOfWork.PasswordResetTokens.DeleteAsync(token, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new OperationResult { HttpStatusCode = HttpStatusCode.OK };
    }
    public async Task<OperationResult> LogoutAsync(LogoutCommand request, CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.UserSessions.GetAsync(
             new ActiveUserSessionByJtiSpecification(request.SessionJti),
             cancellationToken);

        if (session != null)
        {
            var refreshToken = await _unitOfWork.RefreshTokens.GetAsync(
                new NotRevokedRefreshTokenBySessionIdSpecification(session.Id),
                cancellationToken);

            if (refreshToken == null)
            {
                return new OperationResult
                {
                    ErrorMessage = $"Refresh token not found for session {session.Id}",
                    ErrorCode = ErrorCodeConstants.UserNotFound,
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            refreshToken.RevokedAt = DateTimeOffset.UtcNow;
            await _unitOfWork.RefreshTokens.UpdateAsync(refreshToken, cancellationToken);

            session.RevokedAt = DateTimeOffset.UtcNow;
            await _unitOfWork.UserSessions.UpdateAsync(session, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new OperationResult { HttpStatusCode = HttpStatusCode.NoContent };
    }
    public async Task<OperationResult> LogoutFromAllDevicesAsync(long userId, CancellationToken cancellationToken)
    {
        var sessions = await _unitOfWork.UserSessions.GetListAsync(
            new ActiveUserSessionByUserIdIncludeRefreshTokenSpecification(userId),
            cancellationToken);

        foreach (var session in sessions)
        {
            session.RefreshToken.RevokedAt = DateTimeOffset.UtcNow;
            session.RevokedAt = DateTimeOffset.UtcNow;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new OperationResult { HttpStatusCode = HttpStatusCode.NoContent };
    }

    public async Task<OperationResult<RefreshTokenResponse>> RefreshAccessTokenAsync(
              Guid sessionJti,
              string refreshToken,
              CancellationToken cancellationToken)
    {
        var activeSession = await _unitOfWork.UserSessions.GetAsync(
            new ActiveUserSessionByJtiIncludingRefreshTokenSpecification(sessionJti),
            cancellationToken);

        if (activeSession == null)
        {
            return new OperationResult<RefreshTokenResponse>
            {
                ErrorMessage = "Invalid session JTI",
                ErrorCode = ErrorCodeConstants.InvalidSessionJti,
                HttpStatusCode = HttpStatusCode.Unauthorized
            };
        }

        if (activeSession.RefreshToken.ExpiresAt < DateTimeOffset.UtcNow)
        {
            activeSession.RevokedAt = DateTimeOffset.UtcNow;
            activeSession.RefreshToken.RevokedAt = DateTimeOffset.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new OperationResult<RefreshTokenResponse>
            {
                ErrorMessage = "Refresh token expired",
                ErrorCode = ErrorCodeConstants.RefreshTokenExpired,
                HttpStatusCode = HttpStatusCode.Unauthorized
            };
        }

        if (!_tokenService.VerifyRefreshToken(refreshToken, activeSession.RefreshToken.TokenHash))
        {
            return new OperationResult<RefreshTokenResponse>
            {
                ErrorMessage = "Invalid refresh token",
                ErrorCode = ErrorCodeConstants.InvalidRefreshToken,
                HttpStatusCode = HttpStatusCode.Unauthorized
            };
        }

        var newSessionJti = Guid.NewGuid();
        var userToken = await _unitOfWork.Users.GetAsync(
            new UserTokenByIdAsNoTrackingSpecification(activeSession.UserId),
            cancellationToken);

        var newAccessToken = _tokenService.GenerateAccessToken(userToken!, newSessionJti);

        activeSession.Jti = newSessionJti;
        activeSession.LastActivityAt = DateTimeOffset.UtcNow;
        activeSession.LastUpdateAt = DateTimeOffset.UtcNow;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new OperationResult<RefreshTokenResponse>
        {
            HttpStatusCode = HttpStatusCode.OK,
            Payload = new RefreshTokenResponse
            {
                AccessToken = newAccessToken
            }
        };
    }
    public async Task<User> RegisterUserAsync(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var newUser = _mapper.Map<User>(command);
        newUser.PasswordHash = _passwordHashingService.Hash(command.Password);

        await _unitOfWork.Users.AddAsync(newUser, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newUser;
    }

    public async Task<string> GenerateAndSaveEmailVerificationTokenAsync(string userEmail, CancellationToken cancellationToken)
    {
        var userMailModel = await _unitOfWork.Users.GetAsync(
            new UserMailModelAsNoTrackingByUserEmailSpecification(userEmail));

        await _unitOfWork.EmailVerificationTokens.DeleteRangeAsync(
            new EmailConfirmationActiveTokenByUserIdSpecification(userMailModel!.UserId));

        var tokenAsString = _shortTokenService.Get6NumbersToken();

        var tokenEntity = new EmailVerificationToken
        {
            UserId = userMailModel!.UserId,
            TokenHash = _shortTokenService.ComputeHash(tokenAsString),
            ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(_tokenSettings.EmailVerificationTokenLifetimeSeconds),
            AttemptsLeft = _tokenSettings.AttemptsLimitToValidateEmailVerificationByOneToken
        };

        await _unitOfWork.EmailVerificationTokens.AddAsync(tokenEntity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return tokenAsString;
    }
    public async Task<string> GenerateAndSavePasswordResetTokenAsync(string userEmail, CancellationToken cancellationToken)
    {
        var userMailModel = await _unitOfWork.Users.GetAsync(new UserMailModelAsNoTrackingByUserEmailSpecification(userEmail), cancellationToken);

        if (userMailModel == null)
            return string.Empty;

        await _unitOfWork.PasswordResetTokens.DeleteRangeAsync(new ActivePasswordResetTokenByUserIdSpecification(userMailModel.UserId));

        var tokenAsString = _shortTokenService.Get6NumbersToken();

        var token = new PasswordResetToken
        {
            UserId = userMailModel.UserId,
            TokenHash = _shortTokenService.ComputeHash(tokenAsString),

            AttemptsLeft = _tokenSettings.AttemptsLimitToValidatePasswordResetByOneToken,
            ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(_tokenSettings.PasswordResetTokenLifetimeSeconds)
        };

        await _unitOfWork.PasswordResetTokens.AddAsync(token, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return tokenAsString;
    }

    public async Task<OperationResult<List<UserSessionListResponseModel>>> GetUserSessionsList(long userId)
    {
        var sessions = await _unitOfWork.UserSessions.GetListAsync(
            new ActiveUserSessionsByUserIdAsNotrackingSpecification(userId));

        if (sessions.IsNullOrEmpty())
        {
            return new OperationResult<List<UserSessionListResponseModel>>()
            {
                Payload = new List<UserSessionListResponseModel>(),
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
        else
        {
            var sessionResponseModels = sessions.Adapt<List<UserSessionListResponseModel>>();
            var response = new OperationResult<List<UserSessionListResponseModel>>()
            {
                Payload = sessionResponseModels,
                HttpStatusCode = HttpStatusCode.OK,
            };
            return response;
        }
    }
}
