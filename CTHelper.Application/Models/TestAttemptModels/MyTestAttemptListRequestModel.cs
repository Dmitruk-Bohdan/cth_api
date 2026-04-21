using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Models.TestAttemptModels
{
    public class MyTestAttemptListRequestModel : PaginatedListRequestModel
    {
        public long UserId { get; set; }
        public string? TestNameFragment { get; set; }
    }
}
