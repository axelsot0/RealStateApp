using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infraestructure.Persistence.Contexts;


namespace RealStateApp.Infrastructure.Persistence.Repositories
{
    public class PropiedadClienteRepository : IPropiedadClienteRepository
    {
        private readonly AppDbContext _context;

        public PropiedadClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PropiedadCliente>> GetByClienteIdAsync(int clienteId)
        {
            return await _context.PropiedadesClientes
                                   .Where(pc => pc.ClienteId == clienteId)
                                   .ToListAsync();
        }

    }
}
