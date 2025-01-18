using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infraestructure.Persistence.Contexts;

namespace RealStateApp.Infraestructure.Persistence.Seeds
{
    public class ClienteDefault
    {
        public static async Task SeedAsync(AppDbContext context, string userId)
        {
            var clienteDefult = new Cliente
            {
                UserId = userId,
                ProfilePhotoURL = "https://www.shutterstock.com/image-photo/handsome-happy-african-american-bearded-260nw-2460702995.jpg"
            };

            var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.UserId == userId);

            if (cliente == null)
            {
                await context.Clientes.AddAsync(clienteDefult);
                await context.SaveChangesAsync();
            }
        }
    }
}
