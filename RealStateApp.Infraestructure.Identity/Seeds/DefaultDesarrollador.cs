using Microsoft.AspNetCore.Identity;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Infraestructure.Identity.Entities;

namespace RealStateApp.Infraestructure.Identity.Seeds
{
    public class DefaultDesarrollador
    {
        public static async Task<string> SeedAsync(UserManager<AppUser> userManager)
        {
            var defaultUser = new AppUser
            {
                FirstName = "Miguel",
                LastName = "Tejeda",
                UserName = "migueltejeda",
                Email = "desarrollador@gmail.com",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                await userManager.AddToRoleAsync(defaultUser, Roles.Desarrollador.ToString());

                user = defaultUser;
            }

            return user.Id;
        }
    }
}
