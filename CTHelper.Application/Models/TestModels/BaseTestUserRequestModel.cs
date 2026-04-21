using CTHelper.Application.UseCases.TestTaking.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTHelper.Application.Models.TestModels
{
    public class BaseTestUserRequestModel
    {
        public long TestId { get; set; }
        public long UserId { get; set; }
    }
}
