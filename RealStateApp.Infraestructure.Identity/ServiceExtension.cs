using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Wrappers;
using RealStateApp.Core.Domain.Settings;
using RealStateApp.Infraestructure.Identity.Contexts;
using RealStateApp.Infraestructure.Identity.Entities;
using RealStateApp.Infraestructure.Identity.Seeds;
using RealStateApp.Infraestructure.Identity.Services;
using System.Text;

namespace RealStateApp.Infraestructure.Identity
{
    public static class ServiceExtension
    {
        public static void AddIdentityInfraestructureLayerWebApp(this IServiceCollection services, IConfiguration config)
        {
            ConfigureContext(services, config);

            #region Identity
            services.Configure<JWTSettings>(config.GetSection("JWTSettings"));

            services.AddIdentityCore<AppUser>()
                    .AddRoles<IdentityRole>()
                    .AddSignInManager()
                    .AddEntityFrameworkStores<IdentityContext>()
                    .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromSeconds(300);
            });

            // Configurar autenticación basada en cookies
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
            })
            .AddCookie(IdentityConstants.ApplicationScheme, options =>
            {
                options.LoginPath = "/Account/Login"; // Ruta a la página de inicio de sesión
                options.AccessDeniedPath = "/Account/AccessDenied"; // Ruta para acceso denegado
                options.ExpireTimeSpan = TimeSpan.FromHours(24); // Tiempo de expiración de las cookies
            });

            #endregion

            #region Services
            services.AddTransient<IAccountServiceWebApp, AccountServiceWebApp>();
            services.AddTransient<IAccountService, AccountServiceWebApp>();
            #endregion
        }


        public static void AddIdentityInfraestructureLayerWebApi(this IServiceCollection services, IConfiguration config)
        {
           ConfigureContext(services, config);

            #region Identity
            services.Configure<JWTSettings>(config.GetSection("JWTSettings"));

            services.AddIdentityCore<AppUser>()
                    .AddRoles<IdentityRole>()
                    .AddSignInManager()
                    .AddEntityFrameworkStores<IdentityContext>()
                    .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromSeconds(300);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = config["JWTSettings:Issuer"],
                    ValidAudience = config["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTSettings:Key"]))
                };

                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },
                    OnChallenge = c =>
                    {
                        c.HandleResponse();
                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("You're not Authorized"));
                        return c.Response.WriteAsync(result);
                    },
                    OnForbidden = c =>
                    {
                        c.Response.StatusCode = 403;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("You're not Authorized to access this resource"));
                        return c.Response.WriteAsync(result);
                    }
                };
            });
            #endregion

            #region Services
            services.AddTransient<IAccountServiceWebApi, AccountServiceWebApi>();
            services.AddTransient<IAccountService, AccountServiceWebApi>();

            #endregion
        }

        public static async Task<IdentitySeedResponse> SeedIdentityDbAsync(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await DefaultRoles.SeedAsync(roleManager);

                    IdentitySeedResponse response = new();

                    response.AdminId =  await DefaultAdmin.SeedAsync(userManager);
                    response.AgenteId =  await DefaultAgente.SeedAsync(userManager);
                    response.ClienteId =  await DefaultCliente.SeedAsync(userManager);
                    response.DesarrolladorId = await DefaultDesarrollador.SeedAsync(userManager);   

                    return response;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        #region Private Methods

        private static void ConfigureContext(IServiceCollection services, IConfiguration config)
        {
            #region Contexts
            if (config.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityContext>(options => options.UseInMemoryDatabase("IdentityDbInMemory"));
            }
            else
            {
                services.AddDbContext<IdentityContext>(options =>
                {
                    options.EnableSensitiveDataLogging();
                    options.UseSqlServer(config.GetConnectionString("IdentityDbConnection"),
                        m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
                });
            }
            #endregion
        }

        #endregion
    }
}
