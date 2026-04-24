using CTHelper.Application.Common.Constants;
using CTHelper.Application.Models;
using CTHelper.Application.Models.TestAttemptModels;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Domain.Common.Enums;
using CTHelper.Domain.Entities;
using CTHelper.Persistence.Context;
using CTHelper.Presentation.Dtos;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class TestAttemptService : ITestAttemptService
    {
        private readonly AppDbContext _dbContext;

        public TestAttemptService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<OperationResult> CancelTestAttempt(CancelTestAttemptRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> CompleteTestAttempt(CompleteTestAttemptRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<TestAttemptDetails>> GetMyAttempt(MyTestAttemptRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<PaginatedListResponseModel<TestAttemptListItemModel>>> GetMyAttemptList(MyTestAttemptListRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<TestAttemptDetails>> GetStudentAttempt(StudentTestAttemptRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<PaginatedListResponseModel<TestAttemptListItemModel>>> GetStudentAttemptList(StudentTestAttemptListRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> PauseTestAttempt(PauseTestAttemptRequestModel requestModel)
        {
            var attemptInfo = await _dbContext.TestAttempts
                .Where(ta =>
                    ta.Id == requestModel.AttemptId
                    && ta.StudentId == requestModel.UserId)
                .Select(ta =>
                new
                {
                    ta.Status,
                    ta.TestId,
                    ta.StudentId,
                    IsExam = ta.Test.IsTraning
                })
                .FirstOrDefaultAsync();


            if(attemptInfo == null)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.AttemptNotFound,
                    ErrorMessage = "Attempt not found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }
            if (attemptInfo.Status != TestAttemptStatusType.InProgress)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.AttemptNotActive,
                    ErrorMessage = "Attempt is not in progress. You can stop only active attempts",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }
            if (attemptInfo.IsExam)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.AttemptIsExaminative,
                    ErrorMessage = "You can not stop examinative attempts",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }


            var attempt = await _dbContext.TestAttempts.FirstOrDefaultAsync(ta => ta.Id == requestModel.AttemptId);

            attempt!.Duration += (int)Math.Ceiling((DateTimeOffset.UtcNow - attempt.LastResumedAt).TotalSeconds);

            attempt.Status = TestAttemptStatusType.Paused;

            await _dbContext.SaveChangesAsync();

            return new OperationResult();
        }

        public Task<OperationResult> ResumeTestAttempt(ResumeTestAttemptRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> StartTestAttempt(StartTestAttemptRequestModel requestModel)
        {
            var testInfo = await _dbContext.Tests
                .Where(t => t.Id == requestModel.TestId && t.IsPublished)
                .Select(t => new
                {
                    t.IsPublic,
                    Assignment = t.StudentAssignments
                        .FirstOrDefault(sa => sa.StudentId == requestModel.UserId && sa.AttemptsLeft > 0)
                })
                .FirstOrDefaultAsync();

            if (testInfo == null)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.TestNotFound,
                    ErrorMessage = "Test not found",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            var hasAccess = testInfo.IsPublic || testInfo.Assignment != null;
            if (!hasAccess)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.OwnershipRequired,
                    ErrorMessage = "You can only modify your own data. This record belongs to someone else",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            var existingActiveAttempt = await _dbContext.TestAttempts
                .AnyAsync(ta => ta.TestId == requestModel.TestId
                             && ta.StudentId == requestModel.UserId
                             && ta.Status == TestAttemptStatusType.InProgress);

            if (existingActiveAttempt)
            {
                return new OperationResult
                {
                    ErrorCode = ErrorCodeConstants.AttemptAlreadyActive,
                    ErrorMessage = "You already have an active attempt for this test",
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var newTestAttempt = new TestAttempt
                {
                    TestId = requestModel.TestId,
                    StudentId = requestModel.UserId,
                    Duration = 0,
                    Status = TestAttemptStatusType.InProgress
                };
                await _dbContext.TestAttempts.AddAsync(newTestAttempt);

                if (testInfo.Assignment != null)
                {
                    testInfo.Assignment.AttemptsLeft--;
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return new OperationResult();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
