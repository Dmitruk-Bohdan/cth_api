using CTHelper.Application.Models;
using MediatR;

namespace CTHelper.Application.UseCases.UserManagment.Command
{
    public record UpdateUserProfileCommand(
        long UserId,
        string? Username) : IRequest<OperationResult>;
}