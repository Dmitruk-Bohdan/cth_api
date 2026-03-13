using CTHelper.Application.Models;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public record LogoutCommand(long UserId, Guid SessionJti) : IRequest<OperationResult>;
