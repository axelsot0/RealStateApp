using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Enums;

namespace RealStateApp.Core.Domain.Entities
{
    public class Oferta : AuditableBaseEntity
    {
        public int ClienteId { get; set; }
        public int PropiedadId { get; set; }
        public decimal Cifra { get; set; }
        public DateTime Created { get; set; }
        public EstadoOferta Estado { get; set; }

        public Cliente Cliente { get; set; }
        public Propiedad Propiedad { get; set; }
    }
}
