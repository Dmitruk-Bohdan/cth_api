using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Specification;
using FluentValidation;

namespace CTHelper.Application.UseCases.Identity.Validation
{
    public class ConfirmEmailVerificationCommandValidation : AbstractValidator<ConfirmEmailVerificationCommand>
    {
        public ConfirmEmailVerificationCommandValidation(IUnitOfWork unitOfWork)
        {
            RuleFor(evv => evv.TokenAsString)
                .Cascade(CascadeMode.Stop)  
                .NotEmpty().WithMessage("Token is required")
                .Length(6).WithMessage("Token must be 6 characters");

            RuleFor(evv => evv.UserId)
                .Cascade(CascadeMode.Stop)
                .MustAsync(async (userId, cancellationToken) =>
                    await unitOfWork.EmailVerificationTokens
                    .ExistsAsync(new EmailConfirmationActiveTokenByUserIdAsNoTrackingSpecification(userId)))
                .WithMessage("No active tokens were found for this user.");
        }
    }
}
