using Microsoft.AspNetCore.Identity;
using RealStateApp.Core.Application.Enums;

namespace RealStateApp.Infraestructure.Identity.Seeds
{
    public class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Cliente.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Agente.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Desarrollador.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
        }
    }
}
