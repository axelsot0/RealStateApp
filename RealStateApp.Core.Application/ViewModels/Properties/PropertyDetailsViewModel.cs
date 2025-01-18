using RealStateApp.Core.Application.ViewModels.Chat;
using RealStateApp.Core.Application.ViewModels.Offers;
using RealStateApp.Core.Application.ViewModels.Agents;
using System.Collections.Generic;

namespace RealStateApp.Core.Application.ViewModels.Properties
{
    public class PropertyDetailsViewModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string TipoPropiedad { get; set; }
        public string TipoVenta { get; set; }
        public decimal Precio { get; set; }
        public decimal Terreno { get; set; }
        public int Habitaciones { get; set; }
        public int Banios { get; set; }
        public string Descripcion { get; set; }
        public List<string> Imagenes { get; set; } = new List<string>();
        public List<string> Mejoras { get; set; }
        public PropertyAgentViewModel Agente { get; set; }
        public List<ChatViewModel> Chats { get; set; } = new List<ChatViewModel>();

        public List<OfferViewModel> Offers { get; set; } 
    }
}
