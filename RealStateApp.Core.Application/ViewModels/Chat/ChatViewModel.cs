using System;
using System.Collections.Generic;

namespace RealStateApp.Core.Application.ViewModels.Chat
{
    public class ChatViewModel
    {
        public int Id { get; set; }
        public string ClienteId { get; set; }
        public string AgenteId { get; set; }
        public int PropiedadId { get; set; }

        public string NombreCliente { get; set; }
        public string NombreAgente { get; set; }
        public List<MensajeViewModel> Mensajes { get; set; } = new List<MensajeViewModel>();
    }
}
