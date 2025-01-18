using Microsoft.AspNetCore.Identity;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Infraestructure.Identity.Entities;

namespace RealStateApp.Infraestructure.Identity.Seeds
{
    public class DefaultAdmin
    {
        public static async Task<string> SeedAsync(UserManager<AppUser> userManager)
        {

            var defaultUser = new AppUser
            {
                FirstName = "Alexander",
                LastName = "Bautista",
                UserName = "alexanderBautista",
                Email = "admin@gmail.com",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());

                user = defaultUser;
            }

            return user.Id;
        }
    }
}
