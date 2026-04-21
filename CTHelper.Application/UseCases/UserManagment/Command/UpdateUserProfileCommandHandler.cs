using CTHelper.Application.Models;
using CTHelper.Application.Models.UserModels;
using CTHelper.Application.Services.Interfaces;
using MapsterMapper;
using MediatR;

namespace CTHelper.Application.UseCases.UserManagment.Command;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, OperationResult>
{
    private readonly IUserManagmentService _userManagmentService;
    private readonly IMapper _mapper;

    public UpdateUserProfileCommandHandler(IUserManagmentService userManagmentService, IMapper mapper)
    {
        _userManagmentService = userManagmentService;
        _mapper = mapper;
    }

    public async Task<OperationResult> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var updateUserProfileModel = _mapper.Map<UpdateUserProfileModel>(request);
        return await _userManagmentService.UpdateUserProfileAsync(updateUserProfileModel);
    }
}