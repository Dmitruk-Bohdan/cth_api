using CTHelper.Application.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTHelper.Application.Models.Group
{
    public class GroupInfoModel
    {
        public long Id {  get; set; }
        public string Name { get; set; } = default!;
        public List<UserProfilePreviewModel> Members { get; set; } = new();


    }
}
