using CTHelper.Application.Models;
using CTHelper.Application.Models.User;
using CTHelper.Application.Services.Interfaces;
using MapsterMapper;
using MediatR;

namespace CTHelper.Application.UseCases.UserManagment.Command;

public class GetUserInfoByIdQueryHandler : IRequestHandler<GetUserInfoByIdQuery, OperationResult<UserProfileResponseModel>>
{
    private readonly IUserManagmentService _userManagmentService;
    private readonly IMapper _mapper;

    public GetUserInfoByIdQueryHandler(IUserManagmentService userManagmentService, IMapper mapper)
    {
        _userManagmentService = userManagmentService;
        _mapper = mapper;
    }

    public async Task<OperationResult<UserProfileResponseModel>> Handle(GetUserInfoByIdQuery request, CancellationToken cancellationToken)
    {
        return await _userManagmentService.GetUserInfoById(request.UserId);
    }
}