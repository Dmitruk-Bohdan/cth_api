using CTHelper.Application.Models;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public record LogoutCommand(Guid SessionJti) : IRequest<OperationResult>;
