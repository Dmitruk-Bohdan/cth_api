namespace CTHelper.Application.Models.Assignment
{
    public class GroupAssignmentDetailsModel : BaseAssignmentDetailsModel
    {
        public long GroupId { get; set; }
        public string GroupName { get; set; } = default!;
    }
}
