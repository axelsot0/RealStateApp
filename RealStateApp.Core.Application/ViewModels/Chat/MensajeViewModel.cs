using System;

namespace RealStateApp.Core.Application.ViewModels.Chat
{
    public class MensajeViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ChatId { get; set; }
        public string UserName { get; set; }
        public string Contenido { get; set; }
        public DateTime Created { get; set; }
    }
}
