using RealStateApp.Core.Application.ViewModels.Sales;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface ISaleTypeService : IGenericService<SaleTypeHandlerViewModel, SaleTypeViewModel, TipoVenta>
    {
        Task<List<SaleTypeViewModel>> GetTiposVentaAsync();     
    }
}