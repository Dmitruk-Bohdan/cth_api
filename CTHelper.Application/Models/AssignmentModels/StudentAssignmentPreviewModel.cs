using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTHelper.Application.Models.AssignmentModels
{
    public class StudentAssignmentPreviewModel
    {
        public long AssignmentId { get; set; }
        public string TeacherName { get; set; } = default!;
        public long TeacherId { get; set; }
        public string TestName { get; set; } = default!;
        public DateTimeOffset ExpiredAt { get; set; }
    }
}
