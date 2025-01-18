using RealStateApp.Core.Application.ViewModels.Agents;
using RealStateApp.Core.Application.ViewModels.Filtros;
using RealStateApp.Core.Domain.Entities;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IAgentService
    {
        Task AddAgent(AddAgentDto dto);
        Task<ICollection<AgentViewModel>> GetAgents(AgentFilter filter);
        Task<PropertyAgentViewModel> GetAgentDetailsByIdAsync(int agentId);
        Task<Agente> GetAgenteByUserIdAsync(string agentId);
        Task<Agente> GetAgenteByUserIdAsync(int agentId);
        Task<EditAgentViewModel> GetAgentProfileAsync(string userId);
        Task UpdateAgentProfileAsync(string userId, EditAgentViewModel model);
    }

}
