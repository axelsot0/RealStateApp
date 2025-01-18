using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infraestructure.Persistence.Contexts;

namespace RealStateApp.Infraestructure.Persistence.Repositories
{
    public class TipoVentaRepository : GenericRepository<TipoVenta>, ITipoVentaRepository
    {
        private readonly AppDbContext _context;

        public TipoVentaRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
