using CTHelper.Application.Common.Constants;
using CTHelper.Application.Common.Helpers;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public class RequestEmailVerificationCommandHandler : IRequestHandler<RequestEmailVerificationCommand, Unit>
{
    private IMapper _mapper;
    private IUnitOfWork _unitOfWork;
    private IHashService _hashService;
    private IEmailService _emailService;

    public RequestEmailVerificationCommandHandler(IMapper mapper, IHashService hashService, IUnitOfWork unitOfWork, IEmailService emailService)
    {
        _mapper = mapper;
        _hashService = hashService;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }
    public async Task<Unit> Handle(RequestEmailVerificationCommand request, CancellationToken cancellationToken)
    {
        var tokenAsString = TokenHelper.Get6NumbersToken();

        EmailVerificationToken token = new()
        {
            UserId = request.UserId,
            TokenHash = _hashService.Get128Hash(tokenAsString),
            ExpiresAt = DateTimeOffset.Now.AddSeconds(ApplicationConstants.EmailVerificationTokenLifetimeSeconds)
        };
        
        await _unitOfWork.EmailVerificationTokens.AddAsync(token);
        await _unitOfWork.SaveChangesAsync();

        await _emailService.SendConfirmationEmailAsync(request.UserEmail, tokenAsString);

        return Unit.Value;
    }
}