using CTHelper.Application.Models;
using MediatR;

namespace CTHelper.Application.UseCases.UserManagment.Command;

public record DeleteAvatarCommand(long UserId) : IRequest<OperationResult>;
