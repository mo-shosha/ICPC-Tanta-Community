using Core.IServices;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace ICPC_Tanta_Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // Access Gmail settings from configuration
            var email = _configuration["GmailSettings:Email"];
            var password = _configuration["GmailSettings:Password"];
            var host = _configuration["GmailSettings:Host"];
            var port = int.Parse(_configuration["GmailSettings:Port"]);

            // Configure SMTP client
            var smtpClient = new SmtpClient(host)
            {
                Port = port,
                Credentials = new NetworkCredential(email, password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(email),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            // Send the email
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
