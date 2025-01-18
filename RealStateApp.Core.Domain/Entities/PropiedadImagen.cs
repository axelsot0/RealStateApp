using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Core.Domain.Entities
{
    public class PropiedadImagen : AuditableBaseEntity
    {
        public int PropiedadId { get; set; }
        public string UrlImagen { get; set; }

        public Propiedad Propiedad { get; set; }

    }
}
