using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace СTHelper.Domain.Entities
{
    public class RefreshToken
    {
        public long Id { get; set; }
        public long SessionId { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public string DeviceId { get; set; } = string.Empty;
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? RevokedAt { get; set; }
    }
}
