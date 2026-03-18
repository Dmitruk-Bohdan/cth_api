using CTHelper.Application.Models;
using MediatR;

namespace CTHelper.Application.UseCases.UserManagment.Command;

public record UpdateAvatarCommand(
    long UserId,
    Stream UserAvatarStream) : IRequest<OperationResult>;