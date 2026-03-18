using CTHelper.Application.UseCases.UserManagment.Command;
using FluentValidation;

namespace CTHelper.Application.UseCases.UserManagment.Validation
{
    internal class DeleteUserCommandValidation : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidation()
        {
            RuleFor(da => da.UserId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("User id is required");
        }
    }
}