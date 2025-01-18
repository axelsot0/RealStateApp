using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infraestructure.Persistence.Contexts;

namespace RealStateApp.Infraestructure.Persistence.Repositories
{
    public class DesarrolladorRepository : GenericRepository<Desarrollador>, IDesarrolladorRepository
    {
        private readonly AppDbContext _context;

        public DesarrolladorRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
