using UnitOfWork.Repositories;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace UnitOfWork.Services
{
    public class EmailSender: IEmailSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false)
        {
            using var client = new SmtpClient();
            var credentials = new NetworkCredential()
            {
                UserName = "GolTabiatSite", // without @gmail.com
                Password = "Goltabiat#15234"
            };

            client.Credentials = credentials;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;

            using var emailMessage = new MailMessage()
            {
                To = { new MailAddress(toEmail) },
                From = new MailAddress("GolTabiatSite@gamil.com"), // with @gmail.com
                Subject = subject,
                Body = message,
                IsBodyHtml = isMessageHtml
            };

            client.Send(emailMessage);

            return Task.CompletedTask;
        }
    }
}
