using Core.IServices;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using System.ComponentModel.DataAnnotations;
using Core.DTO;

namespace ICPC_Tanta_Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(EmailMessageDto message)
        {
            if (string.IsNullOrWhiteSpace(message.To) || !new EmailAddressAttribute().IsValid(message.To))
            {
                throw new ArgumentException("Invalid email address.", nameof(message.To));
            }
            try
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
                    Subject = message.Subject,
                    Body = message.Body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(message.To);

                if (!string.IsNullOrWhiteSpace(message.ReplyTo) && new EmailAddressAttribute().IsValid(message.ReplyTo))
                {
                    mailMessage.ReplyToList.Add(new MailAddress(message.ReplyTo));
                }

                // Send the email
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log the exception (you could use a logging library here)
                throw new Exception("Failed to send email. See inner exception for details.", ex);
            }


        }
    }
}
