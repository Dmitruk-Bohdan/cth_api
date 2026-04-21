using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Models.TestAttemptModels
{
    public class StudentTestAttemptListRequestModel : PaginatedListRequestModel
    {
        public long UserId { get; set; }
        public long StudentId { get; set; }
        public long AttemptId { get; set; }
        public string? TestNameFragment { get; set; }
    }
}
