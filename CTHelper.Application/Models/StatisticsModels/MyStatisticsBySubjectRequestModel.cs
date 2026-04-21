namespace CTHelper.Presentation.Controllers
{
    public class MyStatisticsBySubjectRequestModel
    {
        public long UserId { get; set; }
        public long SubjectId { get; set; }
        public DateTimeOffset? DateFrom { get; set; }
        public DateTimeOffset? DateTo { get; set; }
    }
}
