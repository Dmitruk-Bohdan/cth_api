using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Models.Group
{
    public class MyGroupListRequestModel : PaginatedListRequestModel
    {
        public long TeacherId { get; set; }
        public long SubjectId { get; set; }
        public string? GroupName { get; set; }
    }
}
