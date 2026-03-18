using CTHelper.Application.Models;
using CTHelper.Application.Models.Session;
using CTHelper.Application.Services.Interfaces;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Query;

public class GetMySessionListQueryHandler : IRequestHandler<GetMySessionListQuery, OperationResult<List<UserSessionListResponseModel>>>
{
    private IAuthService _authService;

    public GetMySessionListQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<OperationResult<List<UserSessionListResponseModel>>> Handle(GetMySessionListQuery request, CancellationToken cancellationToken)
    {
        return await _authService.GetUserSessionsList(request.UserId);
    }
}
