using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Domain.Settings;
using RealStateApp.Infraestructure.Shared.Services;

namespace RealStateApp.Infraestructure.Shared
{
    public static class ServiceExtension
    {
        public static void AddSharedInfraestructureLayer(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MailSettings>(config.GetSection("MailSettings"));
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
