using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Infraestructure.Persistence.Contexts;
using RealStateApp.Infraestructure.Persistence.Repositories;
using RealStateApp.Infraestructure.Persistence.Seeds;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Infraestructure.Persistence
{
    public static class ServiceExtension
    {
        public static void AddPersistenceInfraestructureLayer(this IServiceCollection services, IConfiguration config)
        {
            #region Database Connection
            if (config.GetValue<bool>("InMemoryDatabase"))
            {
                services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("DbInMemory"));
            }
            else
            {
                services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("DbConnection"),
                    m => m.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
            }
            #endregion

            #region Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddTransient<IDesarrolladorRepository, DesarrolladorRepository>();
            services.AddTransient<ITipoPropiedadRepository, TipoPropiedadRepository>();
            services.AddTransient<ITipoVentaRepository, TipoVentaRepository>(); 
            services.AddTransient<IMejoraRepository, MejoraRepository>();
            services.AddTransient<IAgenteRepository, AgenteRepository>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IPropiedadRepository, PropiedadRepository>();
            services.AddTransient<IOfferRepository, OfferRepository>();
            services.AddTransient<IChatRepository, ChatRepository>();
            services.AddTransient<IMensajeRepository, MensajeRepository>();
            services.AddTransient<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<IPropiedadClienteRepository, PropiedadClienteRepository>();



            #endregion

        }

        public static async Task SeedDefaulEntitiesAsync(this IServiceProvider serviceProvider, IdentitySeedResponse response)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<AppDbContext>();

                    await ClienteDefault.SeedAsync(context, response.ClienteId);
                    await AgenteDefault.SeedAsync(context, response.AgenteId);
                    await AdminDefault.SeedAsync(context, response.AdminId);
                    await DesarrolladorDefault.SeedAsync(context, response.DesarrolladorId);
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}
