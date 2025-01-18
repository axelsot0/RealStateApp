using RealStateApp.Core.Application.ViewModels.Favorites;
using RealStateApp.Core.Application.ViewModels.Properties;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IFavoriteService
    {
        Task<List<PropiedadViewModel>> GetFavoritesByClienteIdAsync(int clienteId);
        Task AddFavoriteAsync(int clienteId, int propiedadId);
        Task RemoveFavoriteAsync(int clienteId, int propiedadId);
        Task<bool> IsFavoriteAsync(int clienteId, int propiedadId);
    }
}
