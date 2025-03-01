using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

public class EmailSender
{
    private readonly string _smtpServer = "smtp.gmail.com"; // SMTP sunucusu (örneğin Gmail)
    private readonly int _smtpPort = 587; // Port numarası
    private readonly string _smtpUser = "youremail@example.com"; // Gönderen e-posta adresi
    private readonly string _smtpPassword = "yourpassword"; // SMTP şifre

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpUser),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(toEmail);

        using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
        {
            smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
            smtpClient.EnableSsl = true;

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
