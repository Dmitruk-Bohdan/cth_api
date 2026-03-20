using CTHelper.Application.Common.Helpers;
using CTHelper.Application.Models;
using CTHelper.Application.Models.TeacherStudent;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Application.Specification.UserSpecifications;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;
using System.Net;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class TeacherStudentService : ITeacherStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShortTokenService _tokenService;
        public TeacherStudentService(IUnitOfWork unitOfWork, IShortTokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async  Task<OperationResult<CreateInvitationCodeResponseModel>> CreateInvitationCodeAsync(
            long teacherId, short? usesCount, DateTimeOffset? expiredAt)
        {
            var user = await _unitOfWork.Users.GetAsync(new UserByIdAsNoTrackingSpecification(teacherId));
            if (user == null)
            {
                return OperationResultHelper.UserNotFoundTemplate<CreateInvitationCodeResponseModel>(id: teacherId);
            }

            var code = _tokenService.Get9SymbolsBindingCode();

            var newInvitationCode = new InvitationCode()
            {
                Code = code,
                TeacherId = teacherId,
                UsesLeft = usesCount,
                ExpiredAt = expiredAt
            };

            await _unitOfWork.InvitationCodes.AddAsync(newInvitationCode);
            await _unitOfWork.SaveChangesAsync();

            var formattedCode = _tokenService.Format9SymbolsBindingCode(code);
            var result = new OperationResult<CreateInvitationCodeResponseModel>()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Payload = new CreateInvitationCodeResponseModel()
                {
                    CodeId = newInvitationCode.Id,
                    Code = code,
                    TeacherId = teacherId,
                    UsesLeft = usesCount,
                    ExpiredAt = expiredAt
                }
            };

            return result;
        }
    }
}
