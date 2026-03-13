using CTHelper.Application.Models;
using CTHelper.Application.Services.Interfaces;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public class RequestEmailVerificationCommandHandler : IRequestHandler<RequestEmailVerificationCommand, OperationResult>
{
    private IAuthService _authService;
    private IEmailService _emailService;

    public RequestEmailVerificationCommandHandler(
        IEmailService emailService,
        IAuthService authService)
    {
        _emailService = emailService;
        _authService = authService;
    }
    public async Task<OperationResult> Handle(RequestEmailVerificationCommand request, CancellationToken cancellationToken)
    {
        var token = await _authService.GenerateAndSaveEmailVerificationTokenAsync(request.UserEmail, cancellationToken);
        
        await _emailService.SendConfirmationEmailAsync(request.UserEmail, token);
        return new OperationResult { HttpStatusCode = System.Net.HttpStatusCode.Created };
    }
}