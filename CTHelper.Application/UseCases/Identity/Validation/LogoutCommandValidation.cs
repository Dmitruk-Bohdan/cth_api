using CTHelper.Application.UseCases.Identity.Command;
using FluentValidation;

namespace CTHelper.Application.UseCases.Identity.Validation
{
    public class LogoutCommandValidation : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidation()
        {
            RuleFor(x => x.UserId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("UserId is required")
                .GreaterThan(0).WithMessage("UserId must be greater than 0");
        }
    }
}
