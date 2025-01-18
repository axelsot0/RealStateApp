using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infraestructure.Persistence.Contexts;

namespace RealStateApp.Infraestructure.Persistence.Seeds
{
    public class AdminDefault
    {
        public static async Task SeedAsync(AppDbContext context, string userId)
        {
            var adminDefult = new Admin
            {
                UserId = userId,
                Cedula = "000-0000000-0"
            };

            var admin = await context.Admins.FirstOrDefaultAsync(a => a.UserId == userId);

            if (admin == null)
            {
                await context.Admins.AddAsync(adminDefult);
                await context.SaveChangesAsync();
            }
        }
    }
}
