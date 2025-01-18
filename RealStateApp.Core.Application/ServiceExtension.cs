using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RealStateApp.Core.Application.Behaviors;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Services;
using System.Reflection;

namespace RealStateApp.Core.Application
{
    public static class ServiceExtension
    {
        public static void AddApplicationLayerWebApp(this IServiceCollection services)
        {
            AddApplicationLayerGenericConfiguration(services);

            #region Services
            AddApplicationLayerGenericServices(services);

            
            services.AddTransient<IPropertyService, PropertyService>();
            services.AddTransient<IAgentService, AgentService>();
            services.AddTransient<IOfferService, OfferService>();
            services.AddTransient<IChatService, ChatService>();
            services.AddTransient<IFavoriteService, FavoriteService>();
            services.AddTransient<ISaleTypeService, SaleTypeService>();
            services.AddTransient<IMejoraService, MejoraService>();
            services.AddTransient<IPropertyTypeService, PropertyTypeService>();
            services.AddTransient<IUploadService, UploadService>();
            
            #endregion
        }

        public static void AddApplicationLayerWebApi(this IServiceCollection services)
        {
            AddApplicationLayerGenericConfiguration(services);
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(options => options.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviors<,>));



            #region Services
            AddApplicationLayerGenericServices(services);
            #endregion
        }

        #region Private Methods

        private static void AddApplicationLayerGenericConfiguration(this IServiceCollection services)
        {
            #region Configurations
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            #endregion
        }

        private static void AddApplicationLayerGenericServices(this IServiceCollection services)
        {
            #region Services
            services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));
            #endregion
        }

        #endregion
    }
}
