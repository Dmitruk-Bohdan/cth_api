using CTHelper.Application.Models;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public record LogoutFromAllDevicesCommand(long UserId) : IRequest<OperationResult>;
