using System.Threading.Tasks;

namespace UnitOfWork.Repositories
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false);
    }
}
