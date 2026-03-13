using CTHelper.Application.Models;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public record RefreshTokenCommand(
    string RefreshToken,
    Guid SessionJti) : IRequest<OperationResult<RefreshTokenResponse>>;

public class RefreshTokenResponse
{
    public string AccessToken { get; set; } = default!;    
}
    
