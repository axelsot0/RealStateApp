using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infraestructure.Persistence.Contexts;

namespace RealStateApp.Infraestructure.Persistence.Repositories
{
    public class MejoraRepository : GenericRepository<Mejora>, IMejoraRepository
    {
        private readonly AppDbContext _context;

        public MejoraRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
