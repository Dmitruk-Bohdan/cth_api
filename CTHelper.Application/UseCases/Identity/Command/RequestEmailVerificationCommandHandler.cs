using CTHelper.Application.Common.Constants;
using CTHelper.Application.Common.Helpers;
using CTHelper.Application.Models;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public class RequestEmailVerificationCommandHandler : IRequestHandler<RequestEmailVerificationCommand, OperationResult>
{
    private IMapper _mapper;
    private IUnitOfWork _unitOfWork;
    private IEmailService _emailService;

    public RequestEmailVerificationCommandHandler(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IEmailService emailService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }
    public async Task<OperationResult> Handle(RequestEmailVerificationCommand request, CancellationToken cancellationToken)
    {
        var tokenAsString = TokenHelper.Get6NumbersToken();

        EmailVerificationToken token = new()
        {
            UserId = request.UserId,
            TokenHash = HashHelper.Get128Hash(tokenAsString),
            ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(ApplicationConstants.EmailVerificationTokenLifetimeSeconds),
            AttemptsLeft = ApplicationConstants.AttemptsLimitToValidateByOneToken

        };
        
        await _unitOfWork.EmailVerificationTokens.AddAsync(token);
        await _unitOfWork.SaveChangesAsync();

        await _emailService.SendConfirmationEmailAsync(request.UserEmail, tokenAsString);

        return new OperationResult();
    }
}