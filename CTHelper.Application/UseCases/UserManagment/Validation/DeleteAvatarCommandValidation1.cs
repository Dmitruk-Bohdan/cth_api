using CTHelper.Application.UseCases.UserManagment.Command;
using FluentValidation;

namespace CTHelper.Application.UseCases.Identity.Validation
{
    public class DeleteAvatarCommandValidation : AbstractValidator<DeleteAvatarCommand>
    {
        public DeleteAvatarCommandValidation()
        {
            RuleFor(da => da.UserId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("User id is required");
        }
    }
}
