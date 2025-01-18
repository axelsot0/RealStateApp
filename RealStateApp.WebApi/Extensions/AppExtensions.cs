using Swashbuckle.AspNetCore.SwaggerUI;

namespace RealStateApp.WebApi.Extensions
{
    public static class AppExtensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app, IEndpointRouteBuilder routeBuilder)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var versionDescriptions = routeBuilder.DescribeApiVersions();

                foreach (var apiVersion in versionDescriptions)
                {
                    var url = $"/swagger/{apiVersion.GroupName}/swagger.json";
                    var name = $"RealEstate API - {apiVersion.GroupName.ToUpperInvariant()}";
                    options.SwaggerEndpoint(url, name);
                }

                options.DefaultModelRendering(ModelRendering.Model);
            });
        }
    }
}
