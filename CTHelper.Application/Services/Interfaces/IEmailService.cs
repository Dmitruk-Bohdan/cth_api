namespace CTHelper.Application.Services.Interfaces
{
    public interface IEmailService
    {
        public Task SendConfirmationEmailAsync(string to, string token);
        public Task SendPasswordResetEmailAsync(string to, string token);
    }
}
