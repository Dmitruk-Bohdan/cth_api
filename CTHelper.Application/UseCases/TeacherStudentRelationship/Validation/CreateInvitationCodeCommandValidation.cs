using CTHelper.Application.UseCases.TeacherStudentRelationship.Command;
using FluentValidation;

namespace CTHelper.Application.UseCases.TeacherStudentRelationship.Validation
{
    internal class CreateInvitationCodeCommandValidation : AbstractValidator<CreateInvitationCodeCommand>
    {
        public CreateInvitationCodeCommandValidation()
        {
            RuleFor(cic => cic.TeacherId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Teacher id is required");

            RuleFor(cic => cic.UsesCount)
                .Must(uc => uc == null || uc > 0)
                .WithMessage("Optional 'uses count' parameter must have a positive value");

            RuleFor(cic => cic.ExpiredAt)
                .Must(ea => ea == null || ea > DateTimeOffset.UtcNow)
                .WithMessage("Optional 'expired at' parameter must have future date value");
        }
    }
}
