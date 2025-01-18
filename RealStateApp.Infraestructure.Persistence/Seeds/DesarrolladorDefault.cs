using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infraestructure.Persistence.Contexts;

namespace RealStateApp.Infraestructure.Persistence.Seeds
{
    public class DesarrolladorDefault
    {
        public static async Task SeedAsync(AppDbContext context, string userId)
        {
            var desarrolladorDefult = new Desarrollador
            {
                UserId = userId,
                Cedula = "000-0000000-1"
            };

            var desarrolador = await context.Desarrolladores.FirstOrDefaultAsync(a => a.UserId == userId);

            if (desarrolador == null)
            {
                await context.Desarrolladores.AddAsync(desarrolladorDefult);
                await context.SaveChangesAsync();
            }
        }
    }
}
