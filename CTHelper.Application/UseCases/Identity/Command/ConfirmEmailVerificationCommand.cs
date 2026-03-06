using CTHelper.Application.Models;
using CTHelper.Domain.Entities;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public record ConfirmEmailVerificationCommand
(
    long UserId,
    string TokenAsString
) : IRequest<OperationResult>;