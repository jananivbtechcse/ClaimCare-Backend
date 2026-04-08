using System.Net;
using System.Net.Mail;

public class EmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            EnableSsl = true,
            Credentials = new NetworkCredential(
                "jananivbtechcse@gmail.com",
                "uswh hobv ggfm uzjs"
            )
        };

        var message = new MailMessage
        {
            From = new MailAddress("jananivbtechcse@gmail.com"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        message.To.Add(toEmail);

        await smtpClient.SendMailAsync(message);
    }
}