using CTHelper.Application.Models;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;
public record RequestEmailVerificationCommand(
    string UserEmail) : IRequest<OperationResult>;
