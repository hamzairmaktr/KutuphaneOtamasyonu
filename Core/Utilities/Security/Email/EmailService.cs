using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendAsync(string to, string subject, string body, bool isHtml = true)
        {
            var smtpSettings = _configuration.GetSection("EmailSettings");
            var host = smtpSettings["Host"];
            var port = smtpSettings["Port"];
            var username = smtpSettings["Username"];
            var password = smtpSettings["Password"];
            var enableSsl = smtpSettings["EnableSsl"];
            var fromDisplayName = smtpSettings["FromDisplayName"];

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("Email settings are missing in appsettings.json. Please check 'EmailSettings' section.");
            }

            var smtpClient = new SmtpClient(host)
            {
                Port = int.Parse(port),
                Credentials = new NetworkCredential(username, password),
                EnableSsl = bool.Parse(enableSsl ?? "true"),
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(username, fromDisplayName ?? "System"),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml,
            };

            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
