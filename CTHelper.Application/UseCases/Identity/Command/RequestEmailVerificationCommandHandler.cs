using CTHelper.Application.Common.Constants;
using CTHelper.Application.Common.Helpers;
using CTHelper.Application.Models;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Application.Specification.EmailVerificationTokenSpecifications;
using CTHelper.Application.Specification.UserSpecifications;
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
        var userMailModel = await _unitOfWork.Users.GetAsync(new UserMailModelAsNoTrackingByUserEmailSpecification(request.UserEmail));

        await _unitOfWork.EmailVerificationTokens.DeleteRangeAsync(new EmailConfirmationActiveTokenByUserIdSpecification(userMailModel!.UserId));

        EmailVerificationToken token = new()
        {
            UserId = userMailModel.UserId,
            TokenHash = HashHelper.Get128Hash(tokenAsString),
            ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(ApplicationConstants.EmailVerificationTokenLifetimeSeconds),
            AttemptsLeft = ApplicationConstants.AttemptsLimitToValidateEmailVerificationByOneToken

        };
        
        await _unitOfWork.EmailVerificationTokens.AddAsync(token);
        await _unitOfWork.SaveChangesAsync();

        await _emailService.SendConfirmationEmailAsync(userMailModel!.Email, tokenAsString);

        return new OperationResult { HttpStatusCode = System.Net.HttpStatusCode.Created };
    }
}