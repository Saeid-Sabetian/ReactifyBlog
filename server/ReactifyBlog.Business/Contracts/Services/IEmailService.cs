using System.Threading.Tasks;

namespace ReactifyBlog.Business.Contracts.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
