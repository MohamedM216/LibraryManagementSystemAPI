namespace LibraryManagementSystemAPI.Services;

public class WelcomeEmailService(IEmailService emailService) {
    public async Task SendWelcomeEmailAsync(string toEmail, string username)  {
        string subject = "Welcome to EasyBorrow!";
        string body = @"
        <html>
            <body>
                <h1>Welcome to EasyBorrow!</h1>
                <p>Dear " + username + @",</p>
                <p>Thank you for joining EasyBorrow. We're excited to have you with us.</p>
                <p>...</p>
            </body>
        </html>";

        await emailService.SendEmailAsync(toEmail, subject, body);
    }
}