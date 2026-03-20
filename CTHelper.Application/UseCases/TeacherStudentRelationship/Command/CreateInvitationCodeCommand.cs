using CTHelper.Application.Models;
using CTHelper.Application.Models.TeacherStudent;
using MediatR;

namespace CTHelper.Application.UseCases.TeacherStudentRelationship.Command;

public record CreateInvitationCodeCommand(
    long TeacherId,
    short? UsesCount, 
    DateTimeOffset? ExpiredAt) 
    : IRequest<OperationResult<CreateInvitationCodeResponseModel>>;
