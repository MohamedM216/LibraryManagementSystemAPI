namespace LibraryManagementSystemAPI.Services.BackgroundServices;

public class LateFeeEmailService(IEmailService emailService) {
    public async Task SendLateFeeWarningEmailAsync(string toEmail, string username, decimal lateFeeValue, DateTime invoiceDate) {
        string subject = "Important: Late Fee Notice for Your EasyBorrow Account";
        string body = @"
        <html>
            <body>
                <h1>Late Fee Notice</h1>
                <p>Dear " + username + @",</p>
                <p>We noticed that your payment for the invoice dated " + invoiceDate.ToString("MMMM dd, yyyy") + @" is overdue. 
                As per our policy, a late fee of <strong>" + lateFeeValue.ToString("C") + @"</strong> has been applied to your account.</p>
                <p>...</p>
            </body>
        </html>";

        try 
        {
            await emailService.SendEmailAsync(toEmail, subject, body);
        } catch (Exception ex)
        {
            Console.WriteLine($"ERROR: sending email failed! --- {ex.Message}");
        }
    }
}