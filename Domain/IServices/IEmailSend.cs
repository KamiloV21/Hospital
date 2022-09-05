using System.Threading.Tasks;

namespace Domain.IServices
{
    public interface IEmailSend
    {
        public Task SendEmail(string message, string email);
    }
}
