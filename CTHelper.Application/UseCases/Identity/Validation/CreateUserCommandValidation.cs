using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Common.Enums;
using CTHelper.Domain.Specification;
using FluentValidation;

namespace CTHelper.Application.UseCases.Identity.Validation
{
    public class CreateUserCommandValidation : AbstractValidator<RegisterUserCommand>
    {
        public CreateUserCommandValidation(IUnitOfWork unitOfWork)
        {
            RuleFor(cuc => cuc.Username)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(cuc => cuc.Email)
                .EmailAddress()
                .MustAsync(async (email,cancellationToken) =>
                    !await unitOfWork.Users.ExistsAsync(
                        new UserByEmailSpecification(email), cancellationToken))
                    .WithMessage("User with this email already exists");

            RuleFor(cuc => cuc.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(100);

            RuleFor(cuc => cuc.Role)
                .NotEmpty()
                .IsInEnum()
                .Must(role => role == UserRole.Student || role == UserRole.Teacher);
        }
    }
}
