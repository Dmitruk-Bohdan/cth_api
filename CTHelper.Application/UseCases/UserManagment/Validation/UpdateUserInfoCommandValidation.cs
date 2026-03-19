using CTHelper.Application.UseCases.UserManagment.Command;
using FluentValidation;

namespace CTHelper.Application.UseCases.UserManagment.Validation
{
    public class UpdateUserProfileCommandValidation : AbstractValidator<UpdateUserProfileCommand>
    {
        public UpdateUserProfileCommandValidation()
        {
            RuleFor(x => x)
                .Must(HasAtLeastOneField)
                .WithMessage("At least one field must be provided");

            RuleFor(x => x.Username)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Username));
        }

        private bool HasAtLeastOneField(UpdateUserProfileCommand x)
        {
            return !string.IsNullOrWhiteSpace(x.Username);
        }
    }
}