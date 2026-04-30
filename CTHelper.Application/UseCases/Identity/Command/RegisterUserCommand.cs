using CTHelper.Domain.Common.Enums;
using CTHelper.Domain.Entities;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command
{
    public record RegisterUserCommand(
        string Username,
        string Email,
        string Password,
        UserRoleEnum Role) : IRequest<User>;
}