using CTHelper.Application.Specification.UserSpecifications;
using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Common.Enums;
using FluentValidation;

namespace CTHelper.Application.UseCases.Identity.Validation
{
    public class RefreshTokenCommandValidator: AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(cuc => cuc.RefreshToken)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(cuc => cuc.SessionJti)
                .Must(jwt => jwt != Guid.Empty)
                .WithMessage("Jwt extraction failure");
        }
    }
}
