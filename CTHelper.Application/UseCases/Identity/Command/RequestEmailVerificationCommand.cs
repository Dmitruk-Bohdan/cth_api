using CTHelper.Application.Models;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;
public record RequestEmailVerificationCommand(
    long UserId,
    string UserEmail) : IRequest<OperationResult>;
