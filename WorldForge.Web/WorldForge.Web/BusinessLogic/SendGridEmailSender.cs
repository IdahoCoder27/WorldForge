namespace WorldForge.Web.BusinessLogic
{
    using Microsoft.Extensions.Options;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System.Threading.Tasks;
    using WorldForge.Web.Interfaces;

    public class SendGridOptions
    {
        public string ApiKey { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }

    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridOptions _options;

        public SendGridEmailSender(IOptions<SendGridOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            var client = new SendGridClient(_options.ApiKey);
            var from = new EmailAddress(_options.SenderEmail, _options.SenderName);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlContent: htmlMessage);
            await client.SendEmailAsync(msg);
        }
    }
}
