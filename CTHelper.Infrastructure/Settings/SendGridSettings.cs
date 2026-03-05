namespace CTHelper.Infrastructure.Settings
{
    public class EmailSettings
    {
        public string Host { get; set; } = default!;
        public int Port { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string FromEmail { get; set; } = default!;
        public string FromName { get; set; } = default!;
    }
}
