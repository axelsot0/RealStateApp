using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infraestructure.Persistence.Contexts;
using RealStateApp.Infraestructure.Persistence.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealStateApp.Infrastructure.Persistence.Repositories
{
    public class OfferRepository : GenericRepository<Oferta>, IOfferRepository
    {
        private readonly AppDbContext _context;

        public OfferRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Oferta>> GetByPropertyIdAsync(int propiedadId)
        {
            return await _context.Ofertas
                .Where(o => o.PropiedadId == propiedadId)
                .Include(o => o.Cliente) // Incluir datos del cliente relacionado
                .ToListAsync();
        }

        public async Task<List<Oferta>> GetByClientIdAsync(int clientId)
        {
            return await _context.Ofertas
                .Where(o => o.ClienteId == clientId)
                .Include(o => o.Propiedad) // Incluir datos de la propiedad relacionada
                .ToListAsync();
        }

        public async Task<List<Oferta>> GetByPropertyIdAndClientIdAsync(int propertyId, int clientId)
        {
            return await _context.Ofertas
                .Where(o => o.PropiedadId == propertyId)
                .ToListAsync();
        }

        public async Task<Oferta> GetAcceptedOfferAsync(int propertyId)
        {
            return await _context.Ofertas
                .FirstOrDefaultAsync(o => o.PropiedadId == propertyId && o.Estado == Core.Domain.Enums.EstadoOferta.Aceptada);
        }
    }
}
