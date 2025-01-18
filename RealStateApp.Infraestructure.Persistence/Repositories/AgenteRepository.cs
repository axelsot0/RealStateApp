using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infraestructure.Persistence.Contexts;

namespace RealStateApp.Infraestructure.Persistence.Repositories
{
    public class AgenteRepository : GenericRepository<Agente>, IAgenteRepository
    {
        private readonly AppDbContext _context;

        public AgenteRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Agente>> GetAllWithPropertiesIncludedAsync()
        {
            return await _context.Agentes.Include(a => a.Propiedades).ToListAsync();
        }

        public async Task<Agente?> GetByIdWithPropertiesIncludedAsync(int id)
        {
            return await _context.Agentes.Include(a => a.Propiedades).FirstOrDefaultAsync(a => a.Id == id); 
        }
    }
}
