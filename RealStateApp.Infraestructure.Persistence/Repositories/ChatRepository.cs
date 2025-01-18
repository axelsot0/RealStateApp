using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infraestructure.Persistence.Contexts;
using RealStateApp.Infraestructure.Persistence.Repositories;


namespace RealStateApp.Infrastructure.Persistence.Repositories
{
    public class ChatRepository : GenericRepository<Chat>, IChatRepository
    {
        private readonly AppDbContext _context;

        public ChatRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Chat>> GetChatsByClientIdAsync(string clientId)
        {
            return await _context.Chats
                .Where(c => c.ClienteId == clientId)
                .Include(c => c.Propiedad)
                .Include(c => c.Mensajes)
                .ToListAsync();
        }

        public async Task<List<Chat>> GetChatsByAgentIdAsync(string agentId)
        {
            return await _context.Chats
                .Where(c => c.AgenteId == agentId)
                .Include(c => c.Propiedad)
                .Include(c => c.Mensajes)
                .ToListAsync();
        }

        public async Task<Chat> GetChatByParticipantsAndPropertyAsync(string clienteId, string agenteId, int propiedadId)
        {
            return await _context.Chats
                .Include(c => c.Mensajes)
                .FirstOrDefaultAsync(c =>
                    c.ClienteId == clienteId &&
                    c.AgenteId == agenteId &&
                    c.PropiedadId == propiedadId);
        }
        public async Task<List<Chat>> GetChatsByPropertyIdAsync(int propertyId)
        {
            return await _context.Chats
                .Where(chat => chat.PropiedadId == propertyId)
                .ToListAsync();
        }

    }
}
