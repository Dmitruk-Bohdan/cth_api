using CTHelper.Application.Common.Helpers;
using CTHelper.Application.Models;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;
using CTHelper.Domain.Specification;
using MediatR;
using System.Numerics;

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
        var token = await _unitOfWork.EmailVerificationTokens.GetAsync(new EmailConfirmationActiveTokenByUserIdSpecification(request.UserId));
        
        if(token!.ExpiresAt < DateTimeOffset.UtcNow)
        {
            var response = new OperationResult<User>()
            {
                ErrorMessage = "Token is expired, request a new one",
                ErrorCode = 400,
            };
            await _unitOfWork.EmailVerificationTokens.DeleteAsync(token);
            await _unitOfWork.SaveChangesAsync();

            return response;
        }

        var requestTokenHash = HashHelper.Get128Hash(request.TokenAsString);

        if(token.TokenHash != requestTokenHash)
        {
            var response = new OperationResult<User>()
            {
                ErrorMessage = $"Token doesn't match!, {--token.AttemptsLeft} attempts left!",
                ErrorCode = 400,
            };

            if(token.AttemptsLeft == 0 )
            {
                await _unitOfWork.EmailVerificationTokens.DeleteAsync(token);
            }
            await _unitOfWork.SaveChangesAsync();

            return response;
        }
        else
        {
            var tokenOwner = await _unitOfWork.Users.GetAsync(new UserByIdSpecification(request.UserId));
            tokenOwner!.IsEmailVerified = true;

            await _unitOfWork.EmailVerificationTokens.DeleteAsync(token);

            await _unitOfWork.SaveChangesAsync();

            return new OperationResult();
        }
    }
}