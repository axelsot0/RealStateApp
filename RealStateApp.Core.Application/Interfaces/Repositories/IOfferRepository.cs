using RealStateApp.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Interfaces.Repositories
{
    public interface IOfferRepository : IGenericRepository<Oferta>
    {
        Task<List<Oferta>> GetByPropertyIdAsync(int propertyId); 
        Task<List<Oferta>> GetByClientIdAsync(int clientId); 
        Task<List<Oferta>> GetByPropertyIdAndClientIdAsync(int propertyId, int clientId);
        Task<Oferta> GetAcceptedOfferAsync(int propertyId);
    }
}
