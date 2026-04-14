namespace CTHelper.Application.Models.Favourite
{
    public class MyFavouriteTestListRequestModel
    {
        public long UserId { get; set; }
        public string SearchTerm { get; set; } = default!;
    }
}
