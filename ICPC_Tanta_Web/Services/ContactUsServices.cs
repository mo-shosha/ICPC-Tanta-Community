using Core.DTO;
using Core.IServices;

namespace ICPC_Tanta_Web.Services
{
    public class ContactUsServices : IContactUsServices
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public ContactUsServices(IEmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task HandleContactUsAsync(ContactUsDto dto)
        {
            var supportEmail = _configuration["Support:Email"] ?? "support@example.com";

            var body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <h3>New Contact Us Message</h3>
                    <p><strong>Name:</strong> {dto.Name}</p>
                    <p><strong>Email:</strong> {dto.Email}</p>
                    <p><strong>Subject:</strong> {dto.Subject}</p>
                    <p><strong>Message:</strong></p>
                    <p>{dto.Message}</p>
                </div>";

            await _emailService.SendEmailAsync(new EmailMessageDto
            {
                To = supportEmail,
                Subject = $"Contact Us: {dto.Subject}",
                Body = body,
                ReplyTo = dto.Email
            });
        }
    }
}
