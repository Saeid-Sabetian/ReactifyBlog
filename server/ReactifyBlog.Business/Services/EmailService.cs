using ReactifyBlog.Business.Contracts.Services;
using System.Threading.Tasks;

namespace ReactifyBlog.Business.Services
{
    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(string toEmail, string subject, string message)
        {
            // This is a placeholder for actual email sending logic.
            // In a real application, you would integrate with an email provider like SendGrid, Mailgun, etc.
            Console.WriteLine($"Sending email to: {toEmail}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
            return Task.CompletedTask;
        }
    }
}
