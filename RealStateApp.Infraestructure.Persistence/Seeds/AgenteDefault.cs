using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infraestructure.Persistence.Contexts;

namespace RealStateApp.Infraestructure.Persistence.Seeds
{
    public class AgenteDefault
    {
        public static async Task SeedAsync(AppDbContext context, string userId)
        {
            var agenteDefult = new Agente
            {
                UserId = userId,
                ProfilePhotoURL = "https://rankmyagent.com/upload/user/76777/a7ciYc.JPEG"
            };

            var agente = await context.Agentes.FirstOrDefaultAsync(a => a.UserId == userId);

            if (agente == null)
            {
                await context.Agentes.AddAsync(agenteDefult);
                await context.SaveChangesAsync();
            }
        }
    }
}
