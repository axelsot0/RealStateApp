using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using RealStateApp.Core.Application.Dtos.Email;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Domain.Settings;
using Microsoft.Extensions.Logging;

namespace RealStateApp.Infraestructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<EmailService> _logger;

        // Inyección de dependencias
        public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }

        public async Task SendAsync(EmailRequest request)
        {
            try
            {
                // Construcción del mensaje de correo
                MimeMessage email = new();
                email.Sender = MailboxAddress.Parse($"{_mailSettings.DisplayName} <{_mailSettings.EmailFrom}>");
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;

                BodyBuilder builder = new();
                builder.HtmlBody = request.Body;
                email.Body = builder.ToMessageBody();

                using SmtpClient smtp = new();
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                // Conexión y autenticación con el servidor SMTP
                await smtp.ConnectAsync(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                _logger.LogInformation("Correo electrónico enviado exitosamente a {To}", request.To);
            }
            catch (Exception ex)
            {
                // Logueo del error para facilitar la depuración
                _logger.LogError(ex, "Error al enviar el correo electrónico.");
                throw new Exception("Ocurrió un error al enviar el correo electrónico.");
            }
        }
    }
}
