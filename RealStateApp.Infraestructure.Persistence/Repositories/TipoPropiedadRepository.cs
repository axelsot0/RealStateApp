using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infraestructure.Persistence.Contexts;

namespace RealStateApp.Infraestructure.Persistence.Repositories
{
    public class TipoPropiedadRepository : GenericRepository<TipoPropiedad>, ITipoPropiedadRepository
    {
        private readonly AppDbContext _context;

        public TipoPropiedadRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
