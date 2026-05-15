using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTHelper.Application.Models.AssignmentModels
{
    public class TeacherAssignmentPreviewModel
    {
        public long AssignmentId { get; set; }
        public bool IsGroupAssignment { get; set; }
        public string RecipientName { get; set; } = default!;
        public long RecipientId { get; set; }
        public string TestName { get; set; } = default!;
        public DateTimeOffset ExpiredAt { get; set; }
    }
}
