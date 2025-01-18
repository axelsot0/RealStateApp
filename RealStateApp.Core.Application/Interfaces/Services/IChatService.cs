using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.ViewModels.Chat;
using RealStateApp.Core.Domain.Entities;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task CreateChatAsync(CreateChatViewModel createChatViewModel);
        Task<ChatViewModel> GetChatByParticipantsAndPropertyAsync(string clienteId, string agenteId, int propiedadId);
        Task<List<ChatViewModel>> GetChatsByPropertyIdAsync(int propertyId);
        Task<List<MensajeViewModel>> GetMessagesByChatIdAsync(int chatId);
        Task AddMessageToChatAsync(CreateMessageViewModel messageViewModel);
    }
}
