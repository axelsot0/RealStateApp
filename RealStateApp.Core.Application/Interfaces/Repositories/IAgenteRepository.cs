using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Repositories
{
    public interface IAgenteRepository : IGenericRepository<Agente>
    {
        Task<List<Agente>> GetAllWithPropertiesIncludedAsync();
        Task<Agente?> GetByIdWithPropertiesIncludedAsync(int id);
    }
}
