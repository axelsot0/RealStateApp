using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Offers;
using RealStateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Services
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _offerRepository;

        public OfferService(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        public async Task<List<OfferViewModel>> GetOffersByPropertyIdAsync(int propertyId)
        {
            var offers = await _offerRepository.GetByPropertyIdAsync(propertyId);
            return offers.Select(o => MapToViewModel(o)).ToList();
        }

        public async Task<List<OfferViewModel>> GetOffersByClientIdAsync(int clientId)
        {
            var offers = await _offerRepository.GetByClientIdAsync(clientId);
            return offers.Select(o => MapToViewModel(o)).ToList();
        }

        public async Task<List<OfferViewModel>> GetOffersByPropertyIdAndClientIdAsync(int propertyId, int clientId)
        {
            var offers = await _offerRepository.GetByPropertyIdAndClientIdAsync(propertyId, clientId);
            return offers.Select(o => MapToViewModel(o)).ToList();
        }

        public async Task CreateOfferAsync(OfferViewModel offerViewModel)
        {
            var newOffer = new Oferta
            {
                ClienteId = offerViewModel.ClienteId,
                PropiedadId = offerViewModel.PropiedadId,
                Cifra = offerViewModel.Cifra,
                Created = DateTime.Now,
                Estado = Domain.Enums.EstadoOferta.Pendiente
            };

            await _offerRepository.AddAsync(newOffer);
        }
        public async Task<OfferViewModel> GetByIdAsync(int offerId)
        {
            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null) return null;

            return MapToViewModel(offer);
        }

        public async Task UpdateAsync(OfferViewModel offerViewModel)
        {
            var existingOffer = await _offerRepository.GetByIdAsync(offerViewModel.Id);
            if (existingOffer == null) throw new KeyNotFoundException("Offer not found.");

            existingOffer.Cifra = offerViewModel.Cifra;
            existingOffer.Estado = offerViewModel.Estado;
            existingOffer.Created = offerViewModel.Created;

            await _offerRepository.UpdateAsync(existingOffer, existingOffer.Id);
        }


        public async Task AcceptOfferAsync(int offerId)
        {
            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null) throw new KeyNotFoundException("Offer not found.");

            // Mark the offer as accepted
            offer.Estado = Domain.Enums.EstadoOferta.Aceptada;

            // Reject other offers for the same property
            var otherOffers = await _offerRepository.GetByPropertyIdAsync(offer.PropiedadId);
            foreach (var otherOffer in otherOffers.Where(o => o.Id != offerId))
            {
                otherOffer.Estado = Domain.Enums.EstadoOferta.Rechazada;
            }

            await _offerRepository.UpdateAsync(offer,offerId);
        }

        public async Task RejectOfferAsync(int offerId)
        {
            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null) throw new KeyNotFoundException("Offer not found.");

            // Mark the offer as rejected
            offer.Estado = Domain.Enums.EstadoOferta.Rechazada;

            await _offerRepository.UpdateAsync(offer, offerId);
        }

        public async Task<OfferViewModel> GetAcceptedOfferByPropertyIdAsync(int propertyId)
        {
            var acceptedOffer = await _offerRepository.GetAcceptedOfferAsync(propertyId);
            return acceptedOffer != null ? MapToViewModel(acceptedOffer) : null;
        }

        // Helper method to map from Oferta to OfferViewModel
        private OfferViewModel MapToViewModel(Oferta offer)
        {
            return new OfferViewModel
            {
                Id = offer.Id,
                ClienteId = offer.ClienteId,
                PropiedadId = offer.PropiedadId,
                Cifra = offer.Cifra,
                Created = offer.Created,
                Estado = offer.Estado // Assuming EstadoOferta is an enum
            };
        }
    }
}
