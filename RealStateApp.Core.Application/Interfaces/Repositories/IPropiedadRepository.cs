using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Repositories
{
    public interface IPropiedadRepository : IGenericRepository<Propiedad>
    {
        Task<List<Propiedad>> GetAllWithInclude();
        Task<List<Propiedad>> GetAllWithIncludeByAgenteId(int agenteId);
        Task<Propiedad?> GetByCodeWithInclude(string codigo);
        Task<Propiedad?> GetByIdWithInclude(int id);
        Task<List<Propiedad>> GetByIdsAsync(List<int> ids);

        Task<List<Propiedad>> GetFavoritesByClienteIdAsync(int clienteId);
        Task AddImagenAsync(PropiedadImagen imagen);
        Task AddMejoraAsync(PropiedadMejora mejora);
    }
}
