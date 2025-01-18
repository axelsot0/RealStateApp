using Microsoft.AspNetCore.Identity;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Infraestructure.Identity.Entities;

namespace RealStateApp.Infraestructure.Identity.Seeds
{
    public class DefaultCliente
    {
        public static async Task<string> SeedAsync(UserManager<AppUser> userManager)
        {
            var defaultUser = new AppUser
            {
                FirstName = "Samuel",
                LastName = "Linares",
                UserName = "samuellinares",
                Email = "cliente@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "000-000-0001",
                PhoneNumberConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                await userManager.AddToRoleAsync(defaultUser, Roles.Cliente.ToString());

                user = defaultUser;
            }

            return user.Id;
        }
    }
}
