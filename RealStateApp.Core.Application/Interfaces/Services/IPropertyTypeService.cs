using RealStateApp.Core.Application.ViewModels.Properties;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IPropertyTypeService : IGenericService<PropertyTypeHandlerViewModel, PropertyTypeViewModel, TipoPropiedad>
    {
        Task<bool> CreatePropertyType(PropertyTypeViewModel model);

        Task<bool> DeletePropertyType(int id);

        Task<PropertyTypeViewModel> GetPropertyTypeById(int id);

        Task<List<PropertyTypeViewModel>> GetTiposPropiedadAsync();
    }
}