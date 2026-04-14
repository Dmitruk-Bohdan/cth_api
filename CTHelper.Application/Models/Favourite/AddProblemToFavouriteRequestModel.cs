using CTHelper.Presentation.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTHelper.Application.Models.Favourite
{
    public class AddProblemToFavouriteRequestModel
    {
        public long UserId { get; set; }
        public long ProblemId { get; set; }
    }
}
