using WorldForge.Web.Interfaces;

namespace WorldForge.Web.BusinessLogic
{
    public class DevEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine($"[DEV EMAIL] To: {email}\nSubject: {subject}\nBody:\n{htmlMessage}");
            return Task.CompletedTask;
        }
    }
}
