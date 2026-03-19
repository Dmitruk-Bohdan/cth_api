using CTHelper.Application.Models;
using CTHelper.Application.Services.Interfaces;
using MediatR;

namespace CTHelper.Application.UseCases.UserManagment.Command;

public class UpdateUserInfoCommandHandler : IRequestHandler<DeleteUserCommand, OperationResult>
{
    private readonly IUserManagmentService _userManagmentService;

    public UpdateUserInfoCommandHandler(IUserManagmentService userManagmentService)
    {
        _userManagmentService = userManagmentService;
    }

    public async Task<OperationResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        return await _userManagmentService.DeleteUserAsync(request.UserId);
    }
}