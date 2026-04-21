namespace CTHelper.Application.Models.Assignment
{
    public class StudentAssignmentDetailsModel : BaseAssignmentDetailsModel
    {
        public long StudentId { get; set; } 
        public string StudentName { get; set; } = default!;
    }
}
