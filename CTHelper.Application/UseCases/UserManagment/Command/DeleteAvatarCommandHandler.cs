using CTHelper.Application.Models;
using CTHelper.Application.Services.Interfaces;
using MediatR;

namespace CTHelper.Application.UseCases.UserManagment.Command;

public class DeleteAvatarCommandHandler : IRequestHandler<DeleteAvatarCommand, OperationResult>
{
    private readonly IUserManagmentService _userManagmentService;

    public DeleteAvatarCommandHandler(IUserManagmentService userManagmentService)
    {
        _userManagmentService = userManagmentService;
    }

    public async Task<OperationResult> Handle(DeleteAvatarCommand request, CancellationToken cancellationToken)
    {
        return await _userManagmentService.DeleteUserAvatarAsync(request.UserId);
    }
}