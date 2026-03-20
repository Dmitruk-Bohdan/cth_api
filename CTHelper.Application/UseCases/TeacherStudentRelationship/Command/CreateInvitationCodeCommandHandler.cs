using CTHelper.Application.Models;
using CTHelper.Application.Models.TeacherStudent;
using CTHelper.Application.Services.Interfaces;
using MediatR;

namespace CTHelper.Application.UseCases.TeacherStudentRelationship.Command;

public class CreateInvitationCodeCommandHandler : IRequestHandler<CreateInvitationCodeCommand, OperationResult<CreateInvitationCodeResponseModel>>
{
    private readonly ITeacherStudentService _relationService;

    public CreateInvitationCodeCommandHandler(ITeacherStudentService relationService)
    {
        _relationService = relationService;
    }

    public async Task<OperationResult<CreateInvitationCodeResponseModel>> Handle(CreateInvitationCodeCommand request, CancellationToken cancellationToken)
    {
        return await _relationService.CreateInvitationCodeAsync(
            request.TeacherId,
            request.UsesCount,
            request.ExpiredAt);
    }
}