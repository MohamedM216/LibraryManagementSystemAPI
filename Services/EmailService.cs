
using System.Net;
using System.Net.Mail;

namespace LibraryManagementSystemAPI.Services;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        if (string.IsNullOrEmpty(toEmail) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body)) {
            Console.WriteLine("Wrong email info");
            return;
        }
        try 
        {
            var mailMessage = new MailMessage
            {
                Subject = subject,
                From = new MailAddress(configuration["EmailSttings:SenderEmail"]),
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            using (var smtpClient = new SmtpClient(configuration["EmailSttings:SMTPServer"], int.Parse(configuration["EmailSttings:SMTPProt"]))) 
            {
                smtpClient.Credentials = new NetworkCredential(configuration["EmailSttings:SenderEmail"], configuration["EmailSttings:SenderPassword"]);
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(mailMessage);
            }

        } catch (Exception ex)
        {
            Console.WriteLine($"ERROR: sending email failed! --- {ex.Message}");
        }
    }
}