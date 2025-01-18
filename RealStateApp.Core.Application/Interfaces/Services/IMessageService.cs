using RealStateApp.Core.Application.ViewModels.Chat;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IMessageService
    {
        Task<List<MensajeViewModel>> GetMessagesByChatIdAsync(int chatId);

        Task SendMessageAsync(MensajeViewModel model);
    }
}
