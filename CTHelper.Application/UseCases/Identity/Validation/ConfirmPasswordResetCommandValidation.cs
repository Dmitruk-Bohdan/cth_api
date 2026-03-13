using CTHelper.Application.Specification.UserSpecifications;
using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Domain.Abstractions;
using FluentValidation;

namespace CTHelper.Application.UseCases.Identity.Validation
{
    public class ConfirmPasswordResetCommandValidation : AbstractValidator<ConfirmPasswordResetCommand>
    {
        public ConfirmPasswordResetCommandValidation(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MustAsync(async (email, cancellationToken) =>
                    await unitOfWork.Users.ExistsAsync(new UserByEmailAsNoTrackingSpecification(email)))
                .WithMessage("User not found.");

            RuleFor(x => x.Token)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Token is required")
                .Length(6).WithMessage("Token must be 6 characters");

            RuleFor(x => x.NewPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");
        }
    }
}
