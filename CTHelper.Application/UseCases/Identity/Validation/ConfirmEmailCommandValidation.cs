using CTHelper.Application.Specification.UserSpecifications;
using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Domain.Abstractions;
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

            RuleFor(evv => evv.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MustAsync(async (email, cancellationToken) =>
                    await unitOfWork.Users.ExistsAsync(new UserByEmailAsNoTrackingSpecification(email)))
                .WithMessage("User not found.");
        }
    }
}
