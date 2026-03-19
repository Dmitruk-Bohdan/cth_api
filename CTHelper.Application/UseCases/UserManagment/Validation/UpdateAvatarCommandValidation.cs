using CTHelper.Application.UseCases.UserManagment.Command;
using FluentValidation;

namespace CTHelper.Application.UseCases.UserManagment.Validation
{
    public class UpdateAvatarCommandValidation : AbstractValidator<UpdateAvatarCommand>
    {
        public UpdateAvatarCommandValidation()
        {
            RuleFor(da => da.UserId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("User id is required");

            RuleFor(da => da.UserAvatarStream)
                .NotEmpty().WithMessage("New avatar file is required");

            RuleFor(da => da.ContentType)
                .NotEmpty().WithMessage("Content type is required");
        }
    }
}