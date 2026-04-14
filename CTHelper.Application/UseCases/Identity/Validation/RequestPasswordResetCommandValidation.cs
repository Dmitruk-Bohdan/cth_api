using CTHelper.Application.Common.Constants;
using CTHelper.Application.Exceptions;
using CTHelper.Application.Models;
using CTHelper.Application.Specification.UserSpecifications;
using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Domain.Abstractions;
using FluentValidation;

namespace CTHelper.Application.UseCases.Identity.Validation
{
    public class RequestPasswordResetCommandValidation : AbstractValidator<RequestPasswordResetCommand>
    {
        public RequestPasswordResetCommandValidation(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.UserEmail)
                .NotEmpty().WithMessage("Email is required");

            RuleFor(x => x.UserEmail)
                .CustomAsync(async (email, context, ct) =>
                {
                    var userExists = await unitOfWork.Users.ExistsAsync(
                        new UserMailModelAsNoTrackingByUserEmailSpecification(email));

                    if (!userExists)
                    {
                        var error = new OperationResult()
                        {
                            ErrorMessage = $"User {email} doesn't exist",
                            ErrorCode = ErrorCodeConstants.UserNotFound
                        };
                        throw new CustomValidationException(error);
                    }
                });
        }
    }
}
