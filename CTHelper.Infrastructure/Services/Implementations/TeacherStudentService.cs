using CTHelper.Application.Common.Constants;
using CTHelper.Application.Common.Helpers;
using CTHelper.Application.Models;
using CTHelper.Application.Models.TeacherStudent;
using CTHelper.Application.Models.UserModels;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Application.Specification.UserSpecifications;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Common.Enums;
using CTHelper.Domain.Entities;
using CTHelper.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class TeacherStudentService : ITeacherStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShortTokenService _tokenService;
        private readonly AppDbContext _dbContext;
        private readonly IUserManagmentService _userManagmentService;
        private readonly IFileStorageService _fileStorageService;
        public TeacherStudentService(IUnitOfWork unitOfWork, IShortTokenService tokenService, AppDbContext dbContext, IUserManagmentService userManagmentService, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _dbContext = dbContext;
            _userManagmentService = userManagmentService;
            _fileStorageService = fileStorageService;
        }

        public async Task<OperationResult<CreateInvitationCodeResponseModel>> CreateInvitationCodeAsync(
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

        public async Task<OperationResult> RequestBindingWithTeacherByCode(long studentId, string code)
        {
            var dbCodeEntity = await _dbContext.InvitationCodes.FirstOrDefaultAsync(c => c.Code == code);
            if(dbCodeEntity == null)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.BindingCodeNotFound,
                    ErrorMessage = "Teacher-student binding code not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }
            if(dbCodeEntity.IsRevoked == true)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.BindingCodeIsRevoked,
                    ErrorMessage = "Binding code is revoked",
                    HttpStatusCode= HttpStatusCode.BadRequest
                };
            }

            var existingRelationship = await _dbContext.TeacherStudents.FirstOrDefaultAsync(ts => ts.TeacherId == dbCodeEntity.TeacherId && ts.StudentId == studentId && ts.IsDeleted == false);

            if(existingRelationship != null)
            {
                if (existingRelationship.Status == TeacherStudentStatusEnum.Blocked)
                {
                    return new OperationResult()
                    {
                        ErrorCode = ErrorCodeConstants.StudentIsBlocked,
                        ErrorMessage = "You are blocked by required teacher",
                        HttpStatusCode = HttpStatusCode.Forbidden
                    };
                }
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.RelationAlreadyExist,
                    ErrorMessage = "You are already bounded with this teacher",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            var bindingRequest = new BindingRequest()
            {
                CodeId = dbCodeEntity.Id,
                StudentId = studentId,
                CreatedAt = DateTimeOffset.UtcNow
            };
            
            _dbContext.BindingRequests.Add(bindingRequest);
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> AcceptStudentByInvitationCode(long teacherId, long bindingRequestId)
        {
            var bindingRequest = await _dbContext.BindingRequests
                .Include(br => br.Code)
                .FirstOrDefaultAsync(br => br.Id == bindingRequestId);

            if (bindingRequest == null)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.BindingRequestNotFound,
                    ErrorMessage = "Specified binding request not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            if (bindingRequest.Code.TeacherId != teacherId)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You can only modify your own data. This record belongs to someone else",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            if (bindingRequest.IsAccepted)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.BindingRequestAlreadyAccepted,
                    ErrorMessage = "This binding request has already been accepted",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            var newTeacherStudentRelation = new TeacherStudent()
            {
                TeacherId = teacherId,
                StudentId = bindingRequest.StudentId,
                Status = TeacherStudentStatusEnum.Active,
            };

            bindingRequest.IsAccepted = true;

            await _dbContext.AddAsync(newTeacherStudentRelation);
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }
        public async Task<OperationResult> RemoveBindingWithTeacher(long studentId, long teacherId)
        {
            var binding = _dbContext.TeacherStudents.FirstOrDefault(ts => ts.TeacherId == teacherId && ts.StudentId == studentId && !ts.IsDeleted);
            if(binding == null)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.BindingNotFound,
                    ErrorMessage = $"Specified binding not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            if(binding.StudentId != studentId)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = $"You can only modify your own data. This record belongs to someone else",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            binding.IsDeleted = true;
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> RemoveBindingWithStudent(long teacherId, long studentId)
        {
            var binding = _dbContext.TeacherStudents.FirstOrDefault(ts => ts.TeacherId == teacherId
            && ts.StudentId == studentId
            && !ts.IsDeleted);
            if (binding == null)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.BindingNotFound,
                    ErrorMessage = $"Specified binding not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            if (binding.TeacherId != teacherId)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = $"You can only modify your own data. This record belongs to someone else",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            binding.IsDeleted = true;
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> BlockStudent(long teacherId, long studentId)
        {
            var binding = _dbContext.TeacherStudents.FirstOrDefault(
                ts => ts.TeacherId == teacherId
                && ts.StudentId == studentId 
                && !ts.IsDeleted
                && ts.Status != TeacherStudentStatusEnum.Blocked);

            if (binding != null)
            {
                binding.Status = TeacherStudentStatusEnum.Blocked;

            }
            else
            {
                var newTeacherStudentRelation = new TeacherStudent()
                {
                    TeacherId = teacherId,
                    StudentId = studentId,
                    Status = TeacherStudentStatusEnum.Blocked,
                };

                await _dbContext.AddAsync(newTeacherStudentRelation);
            }

            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult> UnblockStudent(long teacherId, long studentId)
        {
            var binding = _dbContext.TeacherStudents.FirstOrDefault(ts => ts.StudentId == studentId
            && ts.TeacherId == teacherId
            && !ts.IsDeleted && ts.Status == TeacherStudentStatusEnum.Blocked);
            if (binding == null)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.BindingNotFound,
                    ErrorMessage = $"Specified binding not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            if (binding.TeacherId != teacherId)
            {
                return new OperationResult()
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = $"You can only modify your own data. This record belongs to someone else",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            binding.IsDeleted = true;
            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public async Task<OperationResult<UserProfileResponseModel>> GetMyStudentInfoById(long teacherId, long studentId)
        {
            var binding = _dbContext.TeacherStudents.FirstOrDefault(
               ts => ts.TeacherId == teacherId
               && ts.StudentId == studentId
               && !ts.IsDeleted
               && ts.Status != TeacherStudentStatusEnum.Blocked);

            if (binding == null)
            {
                return new OperationResult<UserProfileResponseModel>()
                {
                    ErrorCode = ErrorCodeConstants.BindingNotFound,
                    ErrorMessage = $"Specified binding not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            else return await _userManagmentService.GetUserInfoById(studentId);
        }

        public async Task<OperationResult<UserProfileResponseModel>> GetMyTeacherInfoById(long teacherId, long studentId)
        {
            var binding = _dbContext.TeacherStudents.FirstOrDefault(
               ts => ts.TeacherId == teacherId
               && ts.StudentId == studentId
               && !ts.IsDeleted
               && ts.Status != TeacherStudentStatusEnum.Blocked);

            if (binding == null)
            {
                return new OperationResult<UserProfileResponseModel>()
                {
                    ErrorCode = ErrorCodeConstants.BindingNotFound,
                    ErrorMessage = $"Specified binding not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            else return await _userManagmentService.GetUserInfoById(teacherId);
        }

        public async Task<OperationResult<List<UserProfilePreviewModel>>> GetMyTeachersList(long studentId)
        {
            var teacherPreviewList = await _dbContext.TeacherStudents
                .Where(ts => 
                    ts.StudentId == studentId
                    && ts.Status == TeacherStudentStatusEnum.Active
                    && ts.IsDeleted == false)
                .Select(ts => new UserProfilePreviewWithAvatarIdModel()
                {
                    UserId = ts.TeacherId,
                    Username = ts.Teacher.Username,
                    AvatarId = ts.Teacher.AvatarImageId
                })
                .ToListAsync();

            var previewTaskList = teacherPreviewList.Select(async (tp) => new UserProfilePreviewModel()
            {
                UserId = tp.UserId,
                Username = tp.Username,
                AvatarUrl = tp.AvatarId == null ? null : await _fileStorageService.GetDownloadUrl(tp.AvatarId!.Value)
            }).ToList();

            var response = (await Task.WhenAll(previewTaskList)).ToList();

            return new OperationResult<List<UserProfilePreviewModel>>()
            {
                Payload = response
            };
        }

        public async Task<OperationResult<List<UserProfilePreviewModel>>> GetMyStudentsList(long teacherId)
        {
            var teacherPreviewList = await _dbContext.TeacherStudents
                .Where(ts =>
                    ts.TeacherId == teacherId
                    && ts.Status == TeacherStudentStatusEnum.Active
                    && ts.IsDeleted == false)
                .Select(ts => new UserProfilePreviewWithAvatarIdModel()
                {
                    UserId = ts.StudentId,
                    Username = ts.Student.Username,
                    AvatarId = ts.Student.AvatarImageId
                })
                .ToListAsync();

            var previewTaskList = teacherPreviewList.Select(async (tp) => new UserProfilePreviewModel()
            {
                UserId = tp.UserId,
                Username = tp.Username,
                AvatarUrl = tp.AvatarId == null ? null : await _fileStorageService.GetDownloadUrl(tp.AvatarId!.Value)
            }).ToList();

            var response = (await Task.WhenAll(previewTaskList)).ToList();

            return new OperationResult<List<UserProfilePreviewModel>>()
            {
                Payload = response
            };
        }
        public async Task<OperationResult<List<UserProfilePreviewModel>>> GetBlockedStudentList(long teacherId)
        {
            var teacherPreviewList = await _dbContext.TeacherStudents
                .Where(ts =>
                    ts.TeacherId == teacherId
                    && ts.Status == TeacherStudentStatusEnum.Blocked
                    && ts.IsDeleted == false)
                .Select(ts => new UserProfilePreviewWithAvatarIdModel()
                {
                    UserId = ts.StudentId,
                    Username = ts.Student.Username,
                    AvatarId = ts.Student.AvatarImageId
                })
                .ToListAsync();

            var previewTaskList = teacherPreviewList.Select(async (tp) => new UserProfilePreviewModel()
            {
                UserId = tp.UserId,
                Username = tp.Username,
                AvatarUrl = tp.AvatarId == null ? null : await _fileStorageService.GetDownloadUrl(tp.AvatarId!.Value)
            }).ToList();

            var response = (await Task.WhenAll(previewTaskList)).ToList();

            return new OperationResult<List<UserProfilePreviewModel>>()
            {
                Payload = response
            };
        }

        public async Task<OperationResult<List<BindingRequestResponseModel>>> GetPendingBindingRequests(long teacherId)
        {
            var bindingRequests = await _dbContext.BindingRequests
                .Where(br => br.Code.TeacherId == teacherId && !br.IsAccepted)
                .Select(br => new BindingRequestResponseModel
                {
                    BindingRequestId = br.Id,
                    StudentId = br.StudentId,
                    StudentUsername = br.Student.Username,
                    StudentAvatarId = br.Student.AvatarImageId,
                    CreatedAt = br.CreatedAt
                })
                .OrderByDescending(br => br.CreatedAt)
                .ToListAsync();

            return new OperationResult<List<BindingRequestResponseModel>>()
            {
                Payload = bindingRequests
            };
        }
    }
}
