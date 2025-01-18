using AutoMapper;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Properties;
using RealStateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Services
{
    public class PropertyTypeService : GenericService<PropertyTypeHandlerViewModel, PropertyTypeViewModel, TipoPropiedad>, IPropertyTypeService
    {
        private readonly IMapper _mapper;
        private readonly ITipoPropiedadRepository _propertyTypeRepo;
        private readonly IPropiedadRepository _propertyRepository;


        public PropertyTypeService(ITipoPropiedadRepository propertyTypeRepo, IMapper mapper, IPropiedadRepository propertyService ) : base(propertyTypeRepo, mapper)
        {
            _propertyTypeRepo = propertyTypeRepo;
            _mapper = mapper;
            _propertyRepository = propertyService;
        }

        public async Task<List<PropertyTypeViewModel>> GetTiposPropiedadAsync()
        {
            var tipos = await _propertyTypeRepo.GetAllAsync();
            var properties = await _propertyRepository.GetAllAsync();

            return tipos.Select(tipo => new PropertyTypeViewModel
            {
                Id = tipo.Id,
                Nombre = tipo.Nombre,
                Descripcion = tipo.Descripcion,
                Count = (from p in properties
                         where p.Id == tipo.Id
                         select p
                         ).Count()

            }).ToList();
        }

        public async Task<PropertyTypeViewModel> GetPropertyTypeById(int id)
        {
            var pt = await _propertyTypeRepo.GetByIdAsync(id);

            var model = new PropertyTypeViewModel
            {
                Id = pt.Id,
                Nombre = pt.Nombre,
                Descripcion = pt.Descripcion,
                Count = 0
            };

            return model;
        }

        public async Task<bool> DeletePropertyType(int id)
        {
            var pt = await _propertyTypeRepo.GetByIdAsync(id);
            await _propertyTypeRepo.DeleteAsync(pt);

            return true;
        }

        public async Task<bool> CreatePropertyType(PropertyTypeViewModel model)
        {
            try
            {
                var pt = new TipoPropiedad
                {
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion,
                };

                await _propertyTypeRepo.AddAsync(pt);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
