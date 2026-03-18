using CTHelper.Application.Models;
using MediatR;

namespace CTHelper.Application.UseCases.UserManagment.Command
{
    public record UpdateUserInfoCommand(
        long UserId,
        string? Username,
        Stream? UserAvatarStream) : IRequest<OperationResult>;
}