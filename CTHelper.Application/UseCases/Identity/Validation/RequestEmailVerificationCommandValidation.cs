using CTHelper.Application.Common.Constants;
using CTHelper.Application.Exceptions;
using CTHelper.Application.Models;
using CTHelper.Application.Specification.UserSpecifications;
using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Domain.Abstractions;
using FluentValidation;

namespace CTHelper.Application.UseCases.Identity.Validation
{
    public class RequestEmailVerificationCommandValidation : AbstractValidator<RequestEmailVerificationCommand>
    {
        public RequestEmailVerificationCommandValidation(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.UserEmail)
                .CustomAsync(async (email, context, ct) =>
                {
                    var user = await unitOfWork.Users.GetAsync(
                        new UserIsMailVerifiedModelByUserEmailSpecification(email));

                    if (user == null)
                    {
                        var error = new OperationResult()
                        {
                            ErrorMessage = $"User {email} doesn't exist",
                            ErrorCode = ErrorCodeConstants.UserNotFound
                        };
                        throw new CustomValidationException(error);
                    }

                    if (user.IsEmailVerified)
                    {
                        var error = new OperationResult()
                        {
                            ErrorMessage = $"User {email} email is already verified",
                            ErrorCode = ErrorCodeConstants.EmailIsAlreadyVerified
                        };

                        throw new CustomValidationException(error);
                    }
                });
        }
    }
}
