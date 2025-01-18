using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Enums;

namespace RealStateApp.Core.Domain.Entities
{
    public class Propiedad : AuditableBaseEntity
    {
        public string Codigo { get; set; }
        public int TipoPropiedadId { get; set; }
        public int TipoVentaId { get; set; }
        public decimal Precio { get; set; }
        public decimal Terreno { get; set; }
        public int Habitaciones { get; set; }
        public int Banios { get; set; }
        public string Descripcion { get; set; }
        public int AgenteId { get; set; }
        public DateTime Created { get; set; } 
        public EstadoPropiedad Estado { get; set; }

        public Agente Agente { get; set; }
        public TipoVenta TipoVenta { get; set; }
        public TipoPropiedad TipoPropiedad { get; set; }
        public List<PropiedadMejora> Mejoras { get; set; } = new List<PropiedadMejora>();
        public List<PropiedadImagen> Imagenes { get; set; } = new List<PropiedadImagen>();
        public List<PropiedadCliente> PropiedadClientes { get; set; }
        public List<Oferta> Ofertas { get; set; }
        public List<Chat> Chats { get; set; }
    }
}
