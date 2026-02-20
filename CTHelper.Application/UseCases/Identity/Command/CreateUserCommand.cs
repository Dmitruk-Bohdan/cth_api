using CTHelper.Domain.Common.Enums;
using CTHelper.Domain.Entities;
using MediatR;

public record CreateUserCommand(
    string Username,
    string Email,
    string Password,
    UserRole Role) : IRequest<long>;