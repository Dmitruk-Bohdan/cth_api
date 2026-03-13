using CTHelper.Application.Models;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;
public record ConfirmPasswordResetCommand(
    string Email,
    string Token,
    string NewPassword) : IRequest<OperationResult>;
