using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infraestructure.Persistence.Contexts;
using RealStateApp.Infraestructure.Persistence.Repositories;


namespace RealStateApp.Infrastructure.Persistence.Repositories
{
    public class MensajeRepository : GenericRepository<Mensaje>, IMensajeRepository
    {
        private readonly AppDbContext _context;

        public MensajeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Mensaje>> GetMessagesByChatIdAsync(int chatId)
        {
            return await _context.Mensajes
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.Created) 
                .ToListAsync();
        }

        public async Task AddMessageToChatAsync(Mensaje mensaje)
        {
            // Verifica si el ChatId existe
            var chat = await _context.Chats.Include(c => c.Propiedad).FirstOrDefaultAsync(c => c.Id == mensaje.ChatId);

            if (chat == null)
                throw new Exception($"El chat con ID {mensaje.ChatId} no existe.");

            // Verifica si el UserId es válido para este chat
            if (chat.ClienteId != mensaje.UserId && chat.AgenteId != mensaje.UserId)
                throw new Exception($"El usuario con ID {mensaje.UserId} no es participante del chat con ID {mensaje.ChatId}.");

            // Agrega el mensaje al contexto
            await _context.Mensajes.AddAsync(mensaje);
            await _context.SaveChangesAsync();
        }


    }
}
