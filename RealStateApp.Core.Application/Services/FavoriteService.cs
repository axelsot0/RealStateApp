using AutoMapper;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Favorites;
using RealStateApp.Core.Application.ViewModels.Properties;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IPropiedadRepository _propiedadRepository;
        private readonly IMapper _mapper;
        
        private readonly IPropiedadClienteRepository _propiedadClienteRepository;

        public FavoriteService(IFavoriteRepository favoriteRepository, IPropiedadRepository propiedadRepository, IPropiedadClienteRepository propiedadClienteRepository, IMapper mapper)
        {
            _favoriteRepository = favoriteRepository;
            _propiedadRepository = propiedadRepository;
            _propiedadClienteRepository = propiedadClienteRepository;
            _mapper = mapper;
        }

        public async Task<List<PropiedadViewModel>> GetFavoritesByClienteIdAsync(int clienteId)
        {
            try
            {
                var propiedadesClientes = await _propiedadClienteRepository.GetByClienteIdAsync(clienteId);

                if (propiedadesClientes == null || !propiedadesClientes.Any())
                {
                    return new List<PropiedadViewModel>();
                }

                var propiedadIds = propiedadesClientes.Select(pc => pc.PropiedadId).ToList();

                var propiedadesFavoritas = await _propiedadRepository.GetByIdsAsync(propiedadIds);

                var favoritosViewModel = _mapper.Map<List<PropiedadViewModel>>(propiedadesFavoritas);

                return favoritosViewModel;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener las propiedades favoritas del cliente {clienteId}: {ex.Message}", ex);
            }
        }


        public async Task AddFavoriteAsync(int clienteId, int propiedadId)
        {
            
            if (await _favoriteRepository.IsFavoriteAsync(clienteId, propiedadId))
            {
                throw new InvalidOperationException("La propiedad ya está marcada como favorita.");
            }

            
            var favorite = new PropiedadCliente
            {
                ClienteId = clienteId,
                PropiedadId = propiedadId
            };

            
            await _favoriteRepository.AddFavoriteAsync(favorite);
        }

        public async Task RemoveFavoriteAsync(int clienteId, int propiedadId)
        {
            
            if (!await _favoriteRepository.IsFavoriteAsync(clienteId, propiedadId))
            {
                throw new InvalidOperationException("La propiedad no está marcada como favorita.");
            }

            
            await _favoriteRepository.RemoveFavoriteAsync(clienteId, propiedadId);
        }

        public async Task<bool> IsFavoriteAsync(int clienteId, int propiedadId)
        {
            
            return await _favoriteRepository.IsFavoriteAsync(clienteId, propiedadId);
        }
    }
}
