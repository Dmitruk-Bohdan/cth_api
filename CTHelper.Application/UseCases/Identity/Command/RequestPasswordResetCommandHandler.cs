using CTHelper.Application.Common.Constants;
using CTHelper.Application.Common.Helpers;
using CTHelper.Application.Models;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Application.Specification.PasswordResetToken;
using CTHelper.Application.Specification.UserSpecifications;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace CTHelper.Application.UseCases.Identity.Command;

public class RequestPasswordResetCommandHandler : IRequestHandler<RequestPasswordResetCommand, OperationResult>
{
    private IMapper _mapper;
    private IUnitOfWork _unitOfWork;
    private IEmailService _emailService;

    public RequestPasswordResetCommandHandler(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IEmailService emailService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task<OperationResult> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
    {
        var tokenAsString = TokenHelper.Get6NumbersToken();
        var userMailModel = await _unitOfWork.Users.GetAsync(new UserMailModelAsNoTrackingByUserEmailSpecification(request.UserEmail));

        if (userMailModel == null)
        {
            return new OperationResult { HttpStatusCode = System.Net.HttpStatusCode.OK };
        }

        await _unitOfWork.PasswordResetTokens.DeleteRangeAsync(new ActivePasswordResetTokenByUserIdSpecification(userMailModel.UserId));

        var token = new PasswordResetToken
        {
            UserId = userMailModel.UserId,
            TokenHash = HashHelper.Get128Hash(tokenAsString),
            AttemptsLeft = ApplicationConstants.AttemptsLimitToValidatePasswordResetByOneToken,
            ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(ApplicationConstants.PasswordResetTokenLifetimeSeconds)
        };

        await _unitOfWork.PasswordResetTokens.AddAsync(token);
        await _unitOfWork.SaveChangesAsync();

        await _emailService.SendPasswordResetEmailAsync(userMailModel.Email, tokenAsString);

        return new OperationResult { HttpStatusCode = System.Net.HttpStatusCode.OK };
    }
}
