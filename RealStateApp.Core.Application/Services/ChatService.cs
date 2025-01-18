using AutoMapper;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Chat;
using RealStateApp.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMensajeRepository _mensajeRepository;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public ChatService(IChatRepository chatRepository, IMensajeRepository mensajeRepository, IAccountService accountService, IMapper mapper)
        {
            _chatRepository = chatRepository;
            _mensajeRepository = mensajeRepository;
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task CreateChatAsync(CreateChatViewModel createChatViewModel)
        {
            var chat = _mapper.Map<Chat>(createChatViewModel);
            await _chatRepository.AddAsync(chat);
        }

        public async Task<ChatViewModel> GetChatByParticipantsAndPropertyAsync(string clienteId, string agenteId, int propiedadId)
        {
            
            var chat = await _chatRepository.GetChatByParticipantsAndPropertyAsync(clienteId, agenteId, propiedadId);
            return _mapper.Map<ChatViewModel>(chat);
        }

        public async Task<List<MensajeViewModel>> GetMessagesByChatIdAsync(int chatId)
        {
            var mensajes = await _mensajeRepository.GetMessagesByChatIdAsync(chatId);
            return _mapper.Map<List<MensajeViewModel>>(mensajes);
        }
        public async Task<List<ChatViewModel>> GetChatsByPropertyIdAsync(int propertyId)
        {
            var chats = await _chatRepository.GetChatsByPropertyIdAsync(propertyId);
            return _mapper.Map<List<ChatViewModel>>(chats);
        }

        public async Task AddMessageToChatAsync(CreateMessageViewModel messageViewModel)
        {
            var mensaje = _mapper.Map<Mensaje>(messageViewModel);

            // Aquí manejamos el caso en el que el mensaje es del agente
            if (!string.IsNullOrEmpty(messageViewModel.AgenteId))
            {
                mensaje.UserId = messageViewModel.AgenteId; // Asigna el AgenteId si es un mensaje de agente
            }
            else
            {
                mensaje.UserId = messageViewModel.UserId;  // Si no, usa el UserId del cliente
            }

            if (mensaje.ChatId <= 0)
            {
                throw new InvalidOperationException("El ChatId no puede ser nulo o 0.");
            }

            await _mensajeRepository.AddAsync(mensaje);
        }



    }
}
