using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Agents;
using RealStateApp.Core.Application.ViewModels.Filtros;
using RealStateApp.Core.Application.ViewModels.Properties;
using RealStateApp.Core.Application.ViewModels.Upgrades;
using RealStateApp.Core.Application.ViewModels.User.Admin;
using RealStateApp.Core.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Propiedad> _repository;
        private readonly IGenericRepository<TipoPropiedad> _repositoryTipoPropiedad;
        private readonly IGenericRepository<TipoVenta> _repositoryTipoVenta;
        private readonly IGenericRepository<PropiedadCliente> _propiedadClienteRepository;
        private readonly IAccountServiceWebApp _accountService;
        private readonly IPropiedadRepository _propiedadRepository;
        private readonly IFavoriteService _favorteService;
        private readonly IMejoraRepository _mejoraRepository;
        private readonly IUploadService _uploadService;
        private readonly IAgentService _agentService;

        public PropertyService(IMapper mapper, IGenericRepository<Propiedad> repository, IGenericRepository<TipoPropiedad> repositoryTipoPropiedad, IGenericRepository<TipoVenta> repositoryTipoVenta, IGenericRepository<PropiedadCliente> propiedadClienteRepository, IAccountServiceWebApp accountService, IPropiedadRepository propiedadRepository, IFavoriteService favorteService, IMejoraRepository mejoraRepository, IUploadService uploadService, IAgentService agentService)
        {
            _mapper = mapper;
            _repository = repository;
            _repositoryTipoPropiedad = repositoryTipoPropiedad;
            _repositoryTipoVenta = repositoryTipoVenta;
            _propiedadClienteRepository = propiedadClienteRepository;
            _accountService = accountService;
            _propiedadRepository = propiedadRepository;
            _favorteService = favorteService;
            _mejoraRepository = mejoraRepository;
            _uploadService = uploadService;
            _agentService = agentService;
        }

        public async Task<List<PropertyTypeViewModel>> GetTiposPropiedadAsync()
        {
            var tipos = await _repositoryTipoPropiedad.GetAllAsync();
            var properties = await _propiedadRepository.GetAllAsync();

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

        public async Task<ICollection<PropiedadViewModel>> FiltroPropiedad(HomeFilterViewModel filtros, int clientId)
        {
            IQueryable<Propiedad> query = _repository.GetQuery()
                .Include(p => p.Imagenes)
                .Include(p => p.TipoPropiedad)
                .Include(p => p.TipoVenta);

            
            query = AplicarFiltros(filtros, query);

            List<Propiedad> propiedades;
            ICollection<PropiedadCliente> favoritos = new List<PropiedadCliente>();

            try
            {
                
                propiedades = await query.ToListAsync();

                
                if (clientId > 0)
                {
                    favoritos = await _propiedadClienteRepository.GetQuery()
                        .Where(pc => pc.ClienteId == clientId)
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("Error al obtener propiedades o favoritos.", ex);
            }

            
            var propiedadesViewModel = _mapper.Map<ICollection<PropiedadViewModel>>(propiedades);
           
            dynamic favorite = 0;
            if (clientId > 0)
            {

                favorite = await _propiedadClienteRepository.GetQuery()
                    .Where(pc => pc.ClienteId == clientId)
                    .Select(pc => pc.PropiedadId)
                    .ToListAsync();
                foreach (var propiedad in propiedadesViewModel)
                {
                    propiedad.IsFavorite = favorite.Contains(propiedad.Id);
                }
            }
            

            return propiedadesViewModel;
        }
        public async Task CreatePropertyAsync(Propiedad propiedad, List<IFormFile> imagenes, List<int> mejoraIds)
        {
            
            propiedad.Created = DateTime.Now;
            await _repository.AddAsync(propiedad);

            
            foreach (var imagen in imagenes)
            {
                var imageUrl = await _uploadService.SaveImageAsync(imagen);
                var propiedadImagen = new PropiedadImagen
                {
                    PropiedadId = propiedad.Id,
                    UrlImagen = imageUrl
                };
                await _propiedadRepository.AddImagenAsync(propiedadImagen);
            }

            
            foreach (var mejoraId in mejoraIds)
            {
                var propiedadMejora = new PropiedadMejora
                {
                    PropiedadId = propiedad.Id,
                    MejoraId = mejoraId
                };
                await _propiedadRepository.AddMejoraAsync(propiedadMejora);
            }
        }
        public async Task DeletePropertyAsync(int propertyId)
        {
            
            var propiedad = await _propiedadRepository.GetByIdAsync(propertyId);

            if (propiedad == null)
                throw new KeyNotFoundException("La propiedad no existe.");

            
            await _propiedadRepository.DeleteAsync(propiedad);
        }
        private IQueryable<Propiedad> AplicarFiltros(HomeFilterViewModel filtros, IQueryable<Propiedad> query)
        {
            if (!string.IsNullOrEmpty(filtros.FiltroCodigo?.Codigo))
            {
                query = query.Where(p => p.Codigo == filtros.FiltroCodigo.Codigo);
            }

            if (!string.IsNullOrEmpty(filtros.FiltroCampos?.TipoPropiedad))
            {
                query = query.Where(p => p.TipoPropiedad.Nombre == filtros.FiltroCampos.TipoPropiedad);
            }

            if (filtros.FiltroCampos?.PrecioMin > 0)
            {
                query = query.Where(p => p.Precio >= filtros.FiltroCampos.PrecioMin);
            }

            if (filtros.FiltroCampos?.PrecioMax > 0)
            {
                query = query.Where(p => p.Precio <= filtros.FiltroCampos.PrecioMax);
            }

            if (filtros.FiltroCampos?.Habitaciones > 0)
            {
                query = query.Where(p => p.Habitaciones == filtros.FiltroCampos.Habitaciones);
            }

            if (filtros.FiltroCampos?.Banios > 0)
            {
                query = query.Where(p => p.Banios == filtros.FiltroCampos.Banios);
            }

            return query;
        }
        public async Task<List<PropertyTypeViewModel>> GetTiposVentasAsync()
        {
            var tiposVentas = await _repositoryTipoVenta.GetAllAsync(); 
            return tiposVentas.Select(tv => new PropertyTypeViewModel
            {
                Id = tv.Id,
                Nombre = tv.Nombre,
                Descripcion = tv.Descripcion
            }).ToList();
        }

        public async Task<List<UpgradeViewModel>> GetAllMejorasAsync()
        {
            var mejoras = await _mejoraRepository.GetAllAsync(); 
            return mejoras.Select(m => new UpgradeViewModel
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Descripcion = m.Descripcion
            }).ToList();
        }

        public async Task<List<PropiedadViewModel>> GetAllWithIncludeAsync()
        {
            var propiedades = await _propiedadRepository.GetAllWithInclude();
            return propiedades.Select(prop => _mapper.Map<PropiedadViewModel>(prop)).ToList();
        }

        public async Task<PropertyDetailsViewModel> GetPropertyDetailsByIdAsync(int propertyId)
        {
            var propiedad = await _repository.GetQuery()
                .Include(p => p.TipoPropiedad)
                .Include(p => p.Agente)
                .Include(p => p.Imagenes)
                .Include(p => p.TipoVenta)
                .Include(p => p.Mejoras).ThenInclude(m => m.Mejora)
                .Include(p => p.Imagenes)
                .FirstOrDefaultAsync(p => p.Id == propertyId);

            if (propiedad == null) return null;
            var agent = propiedad.Agente;
            
            var agentViewModel = await _agentService.GetAgentDetailsByIdAsync(agent.Id);

            var property = new PropertyDetailsViewModel
            {
                Id = propiedad.Id,
                Codigo = propiedad.Codigo,
                
                TipoPropiedad = propiedad.TipoPropiedad?.Nombre,
                TipoVenta = propiedad.TipoVenta?.Nombre,
                Precio = propiedad.Precio,
                Terreno = propiedad.Terreno,
                Habitaciones = propiedad.Habitaciones,
                Banios = propiedad.Banios,
                Descripcion = propiedad.Descripcion,
                Mejoras = propiedad.Mejoras.Select(m => m.Mejora.Nombre).ToList(),
                Imagenes = propiedad.Imagenes?.Select(img => img.UrlImagen).ToList() ?? new List<string>()
            };
            property.Agente = new PropertyAgentViewModel();
            property.Agente.Id = agent.Id;
            property.Agente.ProfilePhotoURL = agentViewModel.ProfilePhotoURL;
            property.Agente.FirstName = agentViewModel.FirstName;
            property.Agente.LastName = agentViewModel.LastName;
            property.Agente.Email = agentViewModel.Email;
            property.Agente.PhoneNumber = agentViewModel.PhoneNumber;
            
            

          
          
            property.Imagenes = propiedad.Imagenes?.Select(img => img.UrlImagen).ToList() ?? new List<string>();
            return property;
        }





        public async Task UpdatePropertyAsync(EditPropertyViewModel model)
        {

            var propiedad = await _repository.GetQuery()
        .Include(p => p.Mejoras)
        .Include(p => p.Imagenes)
        .FirstOrDefaultAsync(p => p.Id == model.Id);

            if (propiedad == null)
                throw new KeyNotFoundException("Propiedad no encontrada");
            if (propiedad.Mejoras == null)
            {
                propiedad.Mejoras = new List<PropiedadMejora>();
            }

            propiedad.Mejoras.Clear();
            propiedad.TipoPropiedadId = model.TipoPropiedadId;
            propiedad.TipoVentaId = model.TipoVentaId;
            propiedad.Precio = model.Precio;
            propiedad.Terreno = model.TamañoPropiedad;
            propiedad.Habitaciones = model.Habitaciones;
            propiedad.Banios = model.Banios;
            propiedad.Descripcion = model.Descripcion;

            
            propiedad.Mejoras.Clear(); 
            foreach (var mejoraId in model.MejoraIds)
            {
                propiedad.Mejoras.Add(new PropiedadMejora
                {
                    MejoraId = mejoraId,
                    PropiedadId = propiedad.Id
                });
            }

            if (propiedad.Imagenes == null)
            {
                propiedad.Imagenes = new List<PropiedadImagen>();
            }
            if (model.Imagenes != null && model.Imagenes.Any())
            {
                foreach (var imagen in model.Imagenes)
                {
                    var imageUrl = await _uploadService.SaveImageAsync(imagen); 
                    propiedad.Imagenes.Add(new PropiedadImagen
                    {
                        UrlImagen = imageUrl,
                        PropiedadId = propiedad.Id
                    });
                }
            }

           
            await _repository.UpdateAsync(propiedad, propiedad.Id);
        }

        public async Task<ICollection<PropiedadViewModel>> GetPropertiesByAgentIdAsync(int agentId)
        {
            var propiedades = await _repository.GetQuery()
                .Include(p => p.Imagenes)
                .Include(p => p.TipoPropiedad)
                .Include(p => p.TipoVenta)
                .Where(p => p.AgenteId == agentId)
                .ToListAsync();

            return _mapper.Map<ICollection<PropiedadViewModel>>(propiedades);
        }
    }







}
