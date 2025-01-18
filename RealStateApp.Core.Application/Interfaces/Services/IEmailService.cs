using RealStateApp.Core.Application.Dtos.Email;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
