using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infraestructure.Persistence.Contexts;

namespace RealStateApp.Infraestructure.Persistence.Repositories
{
    public class PropiedadRepository : GenericRepository<Propiedad>, IPropiedadRepository
    {
        private readonly AppDbContext _context;

        public PropiedadRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<List<Propiedad>> GetAllWithInclude()
        {
            return await _context.Propiedades.Include(p => p.TipoPropiedad)
                                             .Include(p => p.TipoVenta)
                                             .Include(p => p.Mejoras)
                                             .Include(p => p.Imagenes)
                                             
                                             .ToListAsync();
        }

        public async Task<Propiedad?> GetByIdWithInclude(int id)
        {
            return await _context.Propiedades.Include(p => p.TipoPropiedad)
                                             .Include(p => p.TipoVenta)
                                             .Include(p => p.Mejoras)
                                             .ThenInclude(m => m.Mejora)
                                             .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Propiedad?> GetByCodeWithInclude(string codigo)
        {
            return await _context.Propiedades.Include(p => p.TipoPropiedad)
                                             .Include(p => p.TipoVenta)
                                             .Include(p => p.Mejoras)
                                             .ThenInclude(m => m.Mejora)
                                             .FirstOrDefaultAsync(p => p.Codigo == codigo);
        }

        public async Task<List<Propiedad>> GetAllWithIncludeByAgenteId(int agenteId)
        {
            return await _context.Propiedades.Where(p => p.AgenteId == agenteId)
                                             .Include(p => p.TipoPropiedad)
                                             .Include(p => p.TipoVenta)
                                             .Include(p => p.Mejoras)
                                             .ThenInclude(m => m.Mejora)
                                             .ToListAsync();
        }
        public async Task<List<Propiedad>> GetFavoritesByClienteIdAsync(int clienteId)
        {
            return await _context.PropiedadesClientes
                .Where(pc => pc.ClienteId == clienteId)
                .Include(pc => pc.Propiedad)
                .ThenInclude(p => p.Imagenes) 
                .Select(pc => pc.Propiedad)
                .ToListAsync();
        }
        public async Task<List<Propiedad>> GetByIdsAsync(List<int> ids)
        {
            return await _context.Propiedades
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();
        }

        public async Task AddImagenAsync(PropiedadImagen imagen)
        {
            // Agregar imagen a la base de datos
            await _context.PropiedadImagenes.AddAsync(imagen);
            await _context.SaveChangesAsync();
        }

        public async Task AddMejoraAsync(PropiedadMejora mejora)
        {
            // Agregar mejora a la base de datos
            await _context.PropiedadMejoras.AddAsync(mejora);
            await _context.SaveChangesAsync();
        }

    }
}
