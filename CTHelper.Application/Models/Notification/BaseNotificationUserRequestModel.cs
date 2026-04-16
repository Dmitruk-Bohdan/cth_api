using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTHelper.Application.Models.Notification
{
    public class BaseNotificationUserRequestModel
    {
        public long NotificationId { get; set; }
        public long UserId { get; set; }
    }
}
