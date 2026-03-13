using CTHelper.Application.Services.Interfaces;
using CTHelper.Infrastructure.Settings;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using CTHelper.Infrastructure.Common.Constants;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class MailHogService : IEmailService
    {
        private EmailSettings _emailSettings;
        private MobileAppSettings _mobileSettings;

        public MailHogService(
            IOptions<EmailSettings> emailSettings,
            IOptions<MobileAppSettings> mobileSettings)
        {
            _emailSettings = emailSettings.Value;
            _mobileSettings = mobileSettings.Value;
        }

        public async Task SendConfirmationEmailAsync(string to, string token)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = EmailTemplates.EmailConfirmationSubject;
            message.Body = new TextPart("html")
            {
                Text = EmailTemplates.EmailConfirmationBody(token, _mobileSettings.ConfirmEmailUrlScheme)
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTlsWhenAvailable);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendPasswordResetEmailAsync(string to, string token)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = EmailTemplates.PasswordResetSubject;
            message.Body = new TextPart("html")
            {
                Text = EmailTemplates.PasswordResetBody(token, _mobileSettings.ResetPasswordUrlScheme)
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTlsWhenAvailable);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
