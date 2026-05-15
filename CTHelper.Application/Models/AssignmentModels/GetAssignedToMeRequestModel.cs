using CTHelper.Presentation.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTHelper.Application.Models.AssignmentModels
{
    public class GetAssignedToMeRequestModel : PaginatedListRequestModel
    {
        public long UserId { get; set; }
    }
}
