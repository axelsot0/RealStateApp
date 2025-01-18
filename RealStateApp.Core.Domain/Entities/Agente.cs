using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Core.Domain.Entities
{
    public class Agente : AuditableBaseEntity
    {
        public string ProfilePhotoURL { get; set; }
        public string UserId { get; set; }
        public List<Propiedad> Propiedades { get; set; }
    }
}
