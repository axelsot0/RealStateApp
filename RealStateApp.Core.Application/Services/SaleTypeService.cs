using AutoMapper;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Properties;
using RealStateApp.Core.Application.ViewModels.Sales;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Services
{
    public class SaleTypeService : GenericService<SaleTypeHandlerViewModel, SaleTypeViewModel, TipoVenta> ,ISaleTypeService
    {
        private readonly ITipoVentaRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPropiedadRepository _propertyRepo;

        public SaleTypeService(ITipoVentaRepository repository, IMapper mapper, IPropiedadRepository propertyRepo) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _propertyRepo = propertyRepo;
        }

        public async Task<List<SaleTypeViewModel>> GetTiposVentaAsync()
        {
            var tipos = await _repository.GetAllAsync();
            var properties = await _propertyRepo.GetAllAsync();

            return tipos.Select(tipo => new SaleTypeViewModel
            {
                Id = tipo.Id,
                Nombre = tipo.Nombre,
                Descripcion = tipo.Descripcion,
                Count = (from p in properties
                         where p.TipoVentaId == tipo.Id
                         select p
                         ).Count()
            }).ToList();
        }
    }
}
