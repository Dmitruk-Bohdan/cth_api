using CTHelper.Application.Models;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public record ConfirmEmailVerificationCommand
(
    string Email,
    string TokenAsString
) : IRequest<OperationResult>;
