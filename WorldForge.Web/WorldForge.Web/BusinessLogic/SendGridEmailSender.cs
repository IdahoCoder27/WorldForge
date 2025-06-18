namespace WorldForge.Web.BusinessLogic
{
    using Microsoft.Extensions.Options;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System.Threading.Tasks;
    using WorldForge.Web.Interfaces;
    using WorldForge.Web.Models;

    public class SendGridOptions
    {
        public string ApiKey { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }

    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridSettings _settings;

        public SendGridEmailSender(IOptions<SendGridSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(_settings.ApiKey);
            var from = new EmailAddress("allen.gillis27@gmail.com", "WorldForge");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
            await client.SendEmailAsync(msg);
        }
    }
}
