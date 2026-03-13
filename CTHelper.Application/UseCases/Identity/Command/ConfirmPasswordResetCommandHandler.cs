using CTHelper.Application.Common.Constants;
using CTHelper.Application.Common.Helpers;
using CTHelper.Application.Models;
using CTHelper.Application.Specification.PasswordResetToken;
using CTHelper.Application.Specification.UserSpecifications;
using CTHelper.Domain.Abstractions;
using MediatR;
using System.Net;

namespace CTHelper.Application.UseCases.Identity.Command;

public class ConfirmPasswordResetCommandHandler : IRequestHandler<ConfirmPasswordResetCommand, OperationResult>
{
    private IUnitOfWork _unitOfWork;

    public ConfirmPasswordResetCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(ConfirmPasswordResetCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetAsync(new UserByEmaiSpecification(request.Email));

        var token = await _unitOfWork.PasswordResetTokens.GetAsync(new ActivePasswordResetTokenByUserEmailSpecification(user!.Id));

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
            await _unitOfWork.PasswordResetTokens.DeleteAsync(token);
            await _unitOfWork.SaveChangesAsync();

            return new OperationResult
            {
                ErrorMessage = "Token is expired, request a new one",
                ErrorCode = ErrorCodeConstants.PasswordResetTokenIsExpired,
                HttpStatusCode = HttpStatusCode.BadRequest
            };
        }

        var requestTokenHash = HashHelper.Get128Hash(request.Token);

        if (token.TokenHash != requestTokenHash)
        {
            token.AttemptsLeft--;

            if (token.AttemptsLeft == 0)
            {
                await _unitOfWork.PasswordResetTokens.DeleteAsync(token);
            }

            await _unitOfWork.SaveChangesAsync();

            return new OperationResult
            {
                ErrorMessage = $"Token doesn't match!, {token.AttemptsLeft} attempts left!",
                ErrorCode = ErrorCodeConstants.WrongPasswordResetToken,
                HttpStatusCode = HttpStatusCode.BadRequest
            };
        }
        else
        {
            user.PasswordHash = HashHelper.Get128Hash(request.NewPassword);
            
            await _unitOfWork.PasswordResetTokens.DeleteAsync(token);

            await _unitOfWork.SaveChangesAsync();

            return new OperationResult
            {
                HttpStatusCode = HttpStatusCode.OK
            };
        }
    }
}
