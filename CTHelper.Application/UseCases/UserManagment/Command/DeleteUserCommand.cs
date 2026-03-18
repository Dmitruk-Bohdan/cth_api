using CTHelper.Application.Models;
using MediatR;

namespace CTHelper.Application.UseCases.UserManagment.Command;

public record DeleteUserCommand(long UserId) : IRequest<OperationResult>;