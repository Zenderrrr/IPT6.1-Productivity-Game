using FocusUp.Application.Interfaces;
using Resend;

namespace FocusUp.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IResend _resend;
        private readonly ILogger<EmailService> _logger;


        public EmailService(IResend resend, ILogger<EmailService> logger)
        {
            _resend = resend;
            _logger = logger;
        }

        public async System.Threading.Tasks.Task SendEmailAsync(string recipient, string subject, string text)
        {
            var message = new EmailMessage
            {
                From = "FocusUp <noreply@focusup.ch>",  
                Subject = subject,
                TextBody = text
            };

            message.To.Add(recipient);

            var response = await _resend.EmailSendAsync(message);

            if (!response.Success)
                throw response.Exception;

            _logger.LogInformation(
                "Resend hat die E-Mail angenommen. E-Mail-ID: {EmailId}.",
                response.Content);
        }
    }
}