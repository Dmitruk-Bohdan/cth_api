using CTHelper.Presentation.Dtos;

namespace CTHelper.Application.Models.Favourite
{
    public class MyFavouriteProblemListRequestModel : PaginatedListRequestModel
    {
        public long UserId { get; set; }
        public long SubjectId { get; set; }
        public string SearchTerm { get; set; } = default!;
    }
}
