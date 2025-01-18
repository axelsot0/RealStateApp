using System.Collections.Generic;
using System.Threading.Tasks;
using RealStateApp.Core.Domain.Entities; // Asegúrate de importar el namespace correcto

namespace RealStateApp.Core.Application.Interfaces.Repositories
{
    public interface IPropiedadClienteRepository
    {
        Task<List<PropiedadCliente>> GetByClienteIdAsync(int clienteId);
    }
}
