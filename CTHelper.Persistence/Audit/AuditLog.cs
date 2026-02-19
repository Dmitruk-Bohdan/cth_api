using CTHelper.Persistence.Common.Enums;

namespace CTHelper.Persistence.Audit
{
    public class AuditLog
    {
        public long Id { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public long EntityId { get; set; }
        public AuditActionEnum Action { get; set; }
        public string Changes { get; set; } = "{}";
        public DateTimeOffset Timestamp { get; set; }
    }
}
