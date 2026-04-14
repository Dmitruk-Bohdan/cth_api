namespace CTHelper.Application.Models.Favourite
{
    public class RemoveProblemFromFavouriteRequestModel
    {
        public long UserId { get; set; }
        public long ProblemId { get; set; }
    }
}
