using RealStateApp.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Interfaces.Repositories
{
    public interface IMensajeRepository : IGenericRepository<Mensaje>
    {
       
        Task<List<Mensaje>> GetMessagesByChatIdAsync(int chatId);

       
        Task AddMessageToChatAsync(Mensaje mensaje);
    }
}
