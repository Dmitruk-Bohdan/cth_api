using CTHelper.Application.Models;
using CTHelper.Application.Specification.RefreshToken;
using CTHelper.Application.Specification.UserSession;
using CTHelper.Domain.Abstractions;
using MediatR;
using System.Net;

namespace CTHelper.Application.UseCases.Identity.Command;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        // Найти сессию по Jti и UserId
        var sessions = await _unitOfWork.UserSessions.GetListAsync(
            new ActiveUserSessionByJtiAndUserIdSpecification(request.UserId, request.SessionJti),
            cancellationToken);

        var session = sessions.FirstOrDefault();

        if (session != null)
        {
            // Отозвать все refresh token'ы для этой сессии
            var refreshTokens = await _unitOfWork.RefreshTokens.GetListAsync(
                new RefreshTokensBySessionIdSpecification(session.Id),
                cancellationToken);

            foreach (var token in refreshTokens.Where(t => t.RevokedAt == null))
            {
                token.RevokedAt = DateTimeOffset.UtcNow;
                await _unitOfWork.RefreshTokens.UpdateAsync(token, cancellationToken);
            }

            // Отозвать сессию
            session.RevokedAt = DateTimeOffset.UtcNow;
            await _unitOfWork.UserSessions.UpdateAsync(session, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new OperationResult { HttpStatusCode = HttpStatusCode.NoContent };
    }
}
