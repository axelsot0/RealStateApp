using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Core.Domain.Entities
{
    public class Cliente : AuditableBaseEntity
    {
        public string ProfilePhotoURL { get; set; }
        public string UserId { get; set; }
        public List<PropiedadCliente> Favoritas { get; set; }
    }
}
