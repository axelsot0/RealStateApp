using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Repositories
{
    public interface IFavoriteRepository : IGenericRepository<PropiedadCliente>
    {
        Task<List<PropiedadCliente>> GetFavoritesByClienteIdAsync(int clienteId);
        Task AddFavoriteAsync(PropiedadCliente favorite);
        Task RemoveFavoriteAsync(int clienteId, int propiedadId);
        Task<bool> IsFavoriteAsync(int clienteId, int propiedadId);
    }
}
