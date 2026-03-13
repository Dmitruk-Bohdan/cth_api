using CTHelper.Application.Models;
using CTHelper.Application.Services.Interfaces;
using MediatR;
using System.Net;

namespace CTHelper.Application.UseCases.Identity.Command;

public class RequestPasswordResetCommandHandler : IRequestHandler<RequestPasswordResetCommand, OperationResult>
{
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;

    public RequestPasswordResetCommandHandler(
        IAuthService authService,
        IEmailService emailService)
    {
        _authService = authService;
        _emailService = emailService;
    }

    public async Task<OperationResult> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
    {
        var token = await _authService.GenerateAndSavePasswordResetTokenAsync(request.UserEmail, cancellationToken);

        await _emailService.SendPasswordResetEmailAsync(request.UserEmail, token);

        return new OperationResult { HttpStatusCode = System.Net.HttpStatusCode.OK };
    }
}