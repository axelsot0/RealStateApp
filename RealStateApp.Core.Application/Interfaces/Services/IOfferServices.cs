using RealStateApp.Core.Application.ViewModels.Offers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IOfferService
    {
        Task<List<OfferViewModel>> GetOffersByPropertyIdAsync(int propertyId); 
        Task<List<OfferViewModel>> GetOffersByClientIdAsync(int clientId); 
        Task<List<OfferViewModel>> GetOffersByPropertyIdAndClientIdAsync(int propertyId, int clientId); 
        Task CreateOfferAsync(OfferViewModel offerViewModel); 
        Task AcceptOfferAsync(int offerId); 
        Task RejectOfferAsync(int offerId); 
        Task<OfferViewModel> GetAcceptedOfferByPropertyIdAsync(int propertyId);
        Task<OfferViewModel> GetByIdAsync(int offerId);
        Task UpdateAsync(OfferViewModel offerViewModel);
    }
}
