using CTHelper.Application.Models;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public record LogoutFromAllDeviCommand(long UserId) : IRequest<OperationResult>;
