using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Agents;
using RealStateApp.Core.Application.ViewModels.Filtros;
using RealStateApp.Core.Domain.Entities;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Services
{
    public class AgentService : IAgentService
    {
        private readonly IAgenteRepository _agenteRepository;
        private readonly IAccountServiceWebApp _accountService;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService; 

        public AgentService(
                        IAgenteRepository agenteRepository,
                        IAccountServiceWebApp accountService,
                        IMapper mapper,
                        IUploadService uploadService) 
        {
            _agenteRepository = agenteRepository;
            _accountService = accountService;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        

        public async Task<ICollection<AgentViewModel>> GetAgents(AgentFilter filter)
        {
            

            var agents = await _accountService.GetAllAgents(filter);
            return agents;
        }

        public async Task AddAgent(AddAgentDto dto)
        {
            
            var agente = new Agente
            {
                ProfilePhotoURL = dto.ProfilePhotoURL,
                UserId = dto.UserId
            };

            
            await _agenteRepository.AddAsync(agente);
        }
        public async Task<Agente> GetAgenteByUserIdAsync(string userId)
        {
            var agente = await _agenteRepository.GetQuery()
                .FirstOrDefaultAsync(a => a.UserId == userId);
            return agente;
        }
        public async Task<Agente> GetAgenteByUserIdAsync(int agentId)
        {
            var agente = await _agenteRepository.GetQuery()
                .FirstOrDefaultAsync(a => a.Id == agentId);
            return agente;
        }
        public async Task<PropertyAgentViewModel> GetAgentDetailsByIdAsync(int agentId)
        {
            
            var agente = await _agenteRepository.GetByIdWithPropertiesIncludedAsync(agentId);

            if (agente == null)
                throw new KeyNotFoundException($"El agente con ID {agentId} no existe.");

            var agentViewModel = _mapper.Map<PropertyAgentViewModel>(agente);

            var userAccount = await _accountService.GetAccountByUserIdAsync(agente.UserId);

            agentViewModel.FirstName = userAccount.FirstName;
            agentViewModel.LastName = userAccount.LastName;
            agentViewModel.Email = userAccount.Email;
            agentViewModel.PhoneNumber = userAccount.PhoneNumber;

            return agentViewModel;
        }


        public async Task<EditAgentViewModel> GetAgentProfileAsync(string userId)
        {
            var agente = await _agenteRepository.GetQuery()
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (agente == null)
                throw new KeyNotFoundException($"El agente con UserId {userId} no existe.");

            var userAccount = await _accountService.GetAccountByUserIdAsync(userId);

            if (userAccount == null)
                throw new KeyNotFoundException($"El usuario con UserId {userId} no existe.");

            var profileViewModel = _mapper.Map<EditAgentViewModel>(agente);
            profileViewModel.Nombre = userAccount.FirstName;
            profileViewModel.Apellido = userAccount.LastName;
            profileViewModel.Telefono = userAccount.PhoneNumber;
            profileViewModel.FotoUrl = agente.ProfilePhotoURL;

            return profileViewModel;
        }



        public async Task UpdateAgentProfileAsync(string agentId, EditAgentViewModel model)
        {
            var agente = await _agenteRepository
        .GetQuery()
        .FirstOrDefaultAsync(a => a.UserId == agentId);

            if (agente == null)
                throw new KeyNotFoundException($"El agente con ID {agentId} no existe.");

            
            if (model.Foto != null)
            {
                var imageUrl = await _uploadService.SaveImageAsync(model.Foto);
                agente.ProfilePhotoURL = imageUrl;
            }

            
            await _agenteRepository.UpdateAsync(agente, agente.Id);

            
            var editUserModel = new EditAgentViewModel
            {
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Telefono = model.Telefono,
                FotoUrl = agente.ProfilePhotoURL,
                
            };

            await _accountService.UpdateAgentAsync(agente.UserId, editUserModel);
        }


    }
}
