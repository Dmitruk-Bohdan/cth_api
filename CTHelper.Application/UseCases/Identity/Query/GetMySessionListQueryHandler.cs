using CTHelper.Application.Models.Dtos.AuthDtos;
using CTHelper.Application.Models.User;
using CTHelper.Application.Specification.RefreshToken;
using CTHelper.Application.Specification.UserSession;
using CTHelper.Domain.Abstractions;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Query;

public class GetMySessionListQueryHandler : IRequestHandler<GetMySessionListQuery, List<UserSessionDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetMySessionListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<UserSessionDto>> Handle(GetMySessionListQuery request, CancellationToken cancellationToken)
    {
        // Получить все сессии пользователя
        var sessions = await _unitOfWork.UserSessions.GetListAsync<UserSessionWithDeviceIdModel>(
            new UserSessionsByUserIdSpecification(request.UserId),
            cancellationToken);

        if (!sessions.Any())
        {
            return new List<UserSessionDto>();
        }

        // Получить ID сессий
        var sessionIds = sessions.Select(s => s.SessionId).ToList();

        // Получить активные refresh токены для этих сессий
        var activeRefreshTokens = await _unitOfWork.RefreshTokens.GetListAsync(
            new ActiveRefreshTokensBySessionIdsSpecification(sessionIds),
            cancellationToken);

        var activeSessionIds = activeRefreshTokens.Select(rt => rt.SessionId).ToHashSet();

        // Создать словарь для быстрого поиска DeviceId по SessionId
        var refreshTokenDict = activeRefreshTokens.ToDictionary(rt => rt.SessionId, rt => rt.DeviceId);

        // Маппинг в DTO
        var sessionDtos = sessions.Select(s => new UserSessionDto
        {
            Jti = s.Jti,
            ClientType = s.ClientType,
            IpAddress = s.IpAddress,
            DeviceInfo = s.DeviceInfo,
            DeviceId = refreshTokenDict.TryGetValue(s.SessionId, out var deviceId) ? deviceId : null,
            LastActivityAt = s.LastActivityAt,
            CreatedAt = s.CreatedAt,
            IsActive = activeSessionIds.Contains(s.SessionId)
        }).ToList();

        return sessionDtos;
    }
}
