using CTHelper.Application.Models;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;
public record RequestPasswordResetCommand(
    string UserEmail) : IRequest<OperationResult>;
