using AngelusTBlog.ViewModel;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace AngelusTBlog.Services
{
    public class EmailService : IBlogEmailSender
    {
        // Setting up a instance of mail settings
        private readonly MailSettings _mailSettings;

        // Injecting the MailSettings
        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendContactEmailAsync(string emailForm, string name, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(_mailSettings.Mail));
            email.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = $"<b>{name}</b> has sent you an email and can be reached at: <b>{emailForm}</b><br/><br/>{htmlMessage}";

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }

        public async Task SendEmailAsync(string emailto, string subject, string htmlMessage)
        {
            var email = new MimeMessage();

            // Sender email
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);

            // Reciever email
            email.To.Add(MailboxAddress.Parse(emailto));

            // Email Subject
            email.Subject = subject;

            // Email Message
            var messageBuilder = new BodyBuilder()
            {
                HtmlBody = htmlMessage
            };
            email.Body = messageBuilder.ToMessageBody();

            // Smtp Client config.
            // Aquiring the MailKit.Net.Smtp package
            using var smtp = new SmtpClient();

            // Setting the host and port
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);

            // Smtp Auth
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

            // Sex the email config
            await smtp.SendAsync(email);

            // Disconnect the smtp
            smtp.Disconnect(true);
        }
    }
}
