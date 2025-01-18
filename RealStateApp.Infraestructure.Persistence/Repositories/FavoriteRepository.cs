using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infraestructure.Persistence.Contexts;
using RealStateApp.Infraestructure.Persistence.Repositories;

namespace RealStateApp.Infrastructure.Persistence.Repositories
{
    public class FavoriteRepository : GenericRepository<PropiedadCliente>, IFavoriteRepository
    {
        private readonly AppDbContext _context;

        public FavoriteRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<PropiedadCliente>> GetFavoritesByClienteIdAsync(int clienteId)
        {
            return await _context.PropiedadesClientes
                                 .Where(pc => pc.ClienteId == clienteId)
                                 .Include(pc => pc.Propiedad)
                                 .ToListAsync();
        }

        public async Task AddFavoriteAsync(PropiedadCliente favorite)
        {
            _context.PropiedadesClientes.Add(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFavoriteAsync(int clienteId, int propiedadId)
        {
            var favorite = await _context.PropiedadesClientes
                                         .FirstOrDefaultAsync(pc => pc.ClienteId == clienteId && pc.PropiedadId == propiedadId);

            if (favorite != null)
            {
                _context.PropiedadesClientes.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsFavoriteAsync(int clienteId, int propiedadId)
        {
            return await _context.PropiedadesClientes
                                 .AnyAsync(pc => pc.ClienteId == clienteId && pc.PropiedadId == propiedadId);
        }
    }
}
