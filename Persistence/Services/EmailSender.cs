using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using Domain.IServices;

namespace Persistence.Services
{
    public class EmailSender : IEmailSend
    {
        public async Task SendEmail(string messageToSend, string email)
        {
            string from = "h2569445@gmail.com";
            string password = "rmcvvmdfrsndznah";
            var to=new MailAddress(email);
            MailMessage msg = new MailMessage();
            msg.Subject = "Your order";
            msg.Body = messageToSend;
            msg.From = new MailAddress(from);
            msg.To.Add(to);

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            NetworkCredential nc = new NetworkCredential(from, password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = nc;

            await smtp.SendMailAsync(msg);
        }
    }
}
