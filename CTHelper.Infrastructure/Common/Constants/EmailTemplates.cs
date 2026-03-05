namespace CTHelper.Infrastructure.Common.Constants;
public static class EmailTemplates
{
    public static string EmailConfirmationSubject = "Email Confirmation";
    public static string EmailConfirmationBody(string token, string deepLink)
    {
        var link = $"{deepLink}?token={token}";
        return $@"
        <html>
            <body>
                <h2>Confirm your email</h2>
                <p>Click the link below to confirm your email address:</p>
                <a href='{link}'>Open App</a>
                <p>Or enter it manually: <strong>{token}</strong></p>
            </body>
        </html>
    ";
    }
}