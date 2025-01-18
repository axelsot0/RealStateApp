using System;
using RealStateApp.Core.Domain.Enums;

namespace RealStateApp.Core.Application.ViewModels.Offers
{
    public class OfferViewModel
    {
        public int Id { get; set; } 
        public int ClienteId { get; set; } 
        public int PropiedadId { get; set; } 

        public decimal Cifra { get; set; }
        public DateTime Created { get; set; } 
        public EstadoOferta Estado { get; set; } 

        
        public string ClienteNombre { get; set; } 
        public string PropiedadCodigo { get; set; } 
    }
}
