using CTHelper.Application.Common.Constants;
using CTHelper.Application.Exceptions;
using CTHelper.Application.Models;
using CTHelper.Application.Models.User;
using CTHelper.Application.Specification.UserSession;
using CTHelper.Application.Specification.UserSpecifications;
using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Domain.Abstractions;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace CTHelper.Application.UseCases.Identity.Validation;

public class LoginCommandValidation : AbstractValidator<LoginCommand>
{
    public LoginCommandValidation(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required");

        RuleFor(x => x.ClientType)
            .IsInEnum()
            .WithMessage("Invalid client type");

        RuleFor(x => x.IpAddress)
            .Must(ip => IPAddress.TryParse(ip, out _))
            .WithMessage("Invalid IP address format")
            .When(x => x.IpAddress != null); 

        RuleFor(x => x.DeviceInfo)
            .Must(BeValidJson)
            .WithMessage("DeviceInfo must be valid JSON")
            .MaximumLength(500)
            .WithMessage("Device info is too long")
            .When(x => x.DeviceInfo != null);

        RuleFor(x => x.DeviceId)
            .MaximumLength(255)
            .WithMessage("Device ID is too long")
            .When(x => x.DeviceId != null);

        RuleFor(x => x)
            .CustomAsync(async (command, context, ct) =>
            {
                var userExists = await unitOfWork.Users.ExistsAsync(
                    new UserIdByEmailAsNoTrackingSpecification(command.Email),
                    ct);

                if (!userExists)
                {
                    var error = new OperationResult()
                    {
                        ErrorMessage = $"User with email {command.Email} doesn't exist",
                        ErrorCode = ErrorCodeConstants.UserNotFound
                    };
                    throw new CustomValidationException(error);
                }
            });
    }
    private bool BeValidJson(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return false;

        try
        {
            JsonDocument.Parse(json);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
