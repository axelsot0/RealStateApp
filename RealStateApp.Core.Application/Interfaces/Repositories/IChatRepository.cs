using RealStateApp.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Interfaces.Repositories
{
    public interface IChatRepository : IGenericRepository<Chat>
    {
        
        Task<List<Chat>> GetChatsByClientIdAsync(string clientId);

        Task<List<Chat>> GetChatsByAgentIdAsync(string agentId);

        Task<Chat?> GetChatByParticipantsAndPropertyAsync(string clientId, string agentId, int propertyId);

        Task<List<Chat>> GetChatsByPropertyIdAsync(int propertyId);

    }
}
