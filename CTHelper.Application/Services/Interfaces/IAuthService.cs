using CTHelper.Application.Models;
using CTHelper.Application.Models.Session;
using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Application.UseCases.Identity.Command.ResponseModels;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<OperationResult<LoginResponseModel>> LoginAsync(LoginCommand request, CancellationToken cancellationToken);
        Task<OperationResult> ConfirmEmailAsync(string email, string token, CancellationToken cancellationToken);
        Task<OperationResult> ConfirmPasswordResetAsync(string email, string tokenAsString, string newPassword, CancellationToken cancellationToken);
        Task<OperationResult> LogoutAsync(LogoutCommand request, CancellationToken cancellationToken);
        Task<OperationResult> LogoutFromAllDevicesAsync(long userId, CancellationToken cancellationToken);
        Task<OperationResult<RefreshTokenResponse>> RefreshAccessTokenAsync(
            Guid sessionJti,
            string refreshToken,
            CancellationToken cancellationToken);
        Task<User> RegisterUserAsync(RegisterUserCommand command, CancellationToken cancellationToken);
        Task<string> GenerateAndSaveEmailVerificationTokenAsync(string userEmail, CancellationToken cancellationToken);
        Task<string> GenerateAndSavePasswordResetTokenAsync(string userEmail, CancellationToken cancellationToken);
        Task<OperationResult<List<UserSessionListResponseModel>>> GetUserSessionsList(long userId);
    }
}