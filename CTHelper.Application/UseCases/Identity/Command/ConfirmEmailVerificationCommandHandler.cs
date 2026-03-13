using CTHelper.Application.Common.Constants;
using CTHelper.Application.Common.Helpers;
using CTHelper.Application.Models;
using CTHelper.Application.Specification.EmailVerificationTokenSpecifications;
using CTHelper.Application.Specification.UserSpecifications;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;
using MediatR;
using System.Net;

namespace CTHelper.Application.UseCases.Identity.Command;

public class ConfirmEmailVerificationCommandHandler : IRequestHandler<ConfirmEmailVerificationCommand, OperationResult>
{
    private IUnitOfWork _unitOfWork;

    public ConfirmEmailVerificationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(ConfirmEmailVerificationCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetAsync(new UserByEmaiSpecification(request.Email));

        if (user == null)
        {
            return new OperationResult
            {
                ErrorMessage = "User not found",
                ErrorCode = ErrorCodeConstants.UserDoesntExist,
                HttpStatusCode = HttpStatusCode.NotFound
            };
        }

        var token = await _unitOfWork.EmailVerificationTokens.GetAsync(new EmailConfirmationActiveTokenByUserEmailSpecification(request.Email, user.Id));

        if (token == null)
        {
            return new OperationResult
            {
                ErrorMessage = "No active tokens were found for this user.",
                ErrorCode = ErrorCodeConstants.WrongEmailVerificationToken,
                HttpStatusCode = HttpStatusCode.BadRequest
            };
        }

        if (token.ExpiresAt < DateTimeOffset.UtcNow)
        {
            var response = new OperationResult<User>()
            {
                ErrorMessage = "Token is expired, request a new one",
                ErrorCode = ErrorCodeConstants.EmailVerificationTokenIsExpired,
                HttpStatusCode = HttpStatusCode.BadRequest
            };
            await _unitOfWork.EmailVerificationTokens.DeleteAsync(token);
            await _unitOfWork.SaveChangesAsync();

            return response;
        }

        var requestTokenHash = HashHelper.Get128Hash(request.TokenAsString);

        if (token.TokenHash != requestTokenHash)
        {
            var response = new OperationResult<User>()
            {
                ErrorMessage = $"Token doesn't match!, {--token.AttemptsLeft} attempts left!",
                ErrorCode = ErrorCodeConstants.WrongEmailVerificationToken,
                HttpStatusCode = HttpStatusCode.BadRequest
            };

            if (token.AttemptsLeft == 0)
            {
                await _unitOfWork.EmailVerificationTokens.DeleteAsync(token);
            }
            await _unitOfWork.SaveChangesAsync();

            return response;
        }
        else
        {
            user.IsEmailVerified = true;

            await _unitOfWork.EmailVerificationTokens.DeleteAsync(token);

            await _unitOfWork.SaveChangesAsync();

            return new OperationResult { HttpStatusCode = HttpStatusCode.Created };
        }
    }
}
