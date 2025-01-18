using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infraestructure.Persistence.Contexts;
using RealStateApp.Core.Application.Interfaces.Repositories;


namespace RealStateApp.Infraestructure.Persistence.Repositories
{
    public class ClientRepository : GenericRepository<Cliente>, IClientRepository
    {
        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context) : base (context)
        {
            _context = context;
        }
    }
}
