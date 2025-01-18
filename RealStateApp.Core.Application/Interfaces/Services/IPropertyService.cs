using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.ViewModels.Filtros;
using RealStateApp.Core.Application.ViewModels.Properties;
using RealStateApp.Core.Application.ViewModels.Upgrades;
using RealStateApp.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IPropertyService
    {
        Task<List<PropertyTypeViewModel>> GetTiposPropiedadAsync();
        
        Task<ICollection<PropiedadViewModel>> FiltroPropiedad(HomeFilterViewModel filtros, int clientId);
        Task<List<PropertyTypeViewModel>> GetTiposVentasAsync();
        Task<List<UpgradeViewModel>> GetAllMejorasAsync();
        Task CreatePropertyAsync(Propiedad propiedad, List<IFormFile> imagenes, List<int> mejoraIds);
        Task<List<PropiedadViewModel>> GetAllWithIncludeAsync();
        Task DeletePropertyAsync(int propertyId);
        Task<PropertyDetailsViewModel> GetPropertyDetailsByIdAsync(int propertyId);
        Task<ICollection<PropiedadViewModel>> GetPropertiesByAgentIdAsync(int agentId);
        Task UpdatePropertyAsync(EditPropertyViewModel model);


    }
}
