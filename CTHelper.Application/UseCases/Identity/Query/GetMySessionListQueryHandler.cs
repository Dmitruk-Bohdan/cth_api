using CTHelper.Application.Models;
using CTHelper.Application.Models.Dtos.AuthDtos;
using CTHelper.Application.Specification.UserSession;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Common.Extensions;
using Mapster;
using MediatR;
using System.Net;

namespace CTHelper.Application.UseCases.Identity.Query;

public class GetMySessionListQueryHandler : IRequestHandler<GetMySessionListQuery, OperationResult<List<UserSessionDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetMySessionListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult<List<UserSessionDto>>> Handle(GetMySessionListQuery request, CancellationToken cancellationToken)
    {
        var sessions = await _unitOfWork.UserSessions.GetListAsync(
            new ActiveUserSessionsByUserIdAsNotrackingSpecification(request.UserId),
            cancellationToken);

        if (sessions.IsNullOrEmpty())
        {
            return new OperationResult<List<UserSessionDto>>()
            {
                Payload = new List<UserSessionDto>(),
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
        else
        {
            var sessionDtos = sessions.Adapt<List<UserSessionDto>>();
            var response = new OperationResult<List<UserSessionDto>>()
            {
                Payload = sessionDtos,
                HttpStatusCode = HttpStatusCode.OK,
            };
            return response;
        }
    }
}
