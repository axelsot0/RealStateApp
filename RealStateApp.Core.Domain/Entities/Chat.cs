using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Core.Domain.Entities
{
    public class Chat : AuditableBaseEntity
    {
        public string ClienteId { get; set; }
        public string AgenteId { get; set; }
        public int PropiedadId { get; set; }

        public Propiedad Propiedad { get; set; }
        public List<Mensaje> Mensajes { get; set; }
    }
}
