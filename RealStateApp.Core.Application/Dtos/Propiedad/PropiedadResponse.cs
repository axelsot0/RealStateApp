using RealStateApp.Core.Application.Dtos.Mejoras;
using RealStateApp.Core.Application.Dtos.TipoPropiedades;
using RealStateApp.Core.Application.Dtos.TipoVentas;
using RealStateApp.Core.Domain.Enums;

namespace RealStateApp.Core.Application.Dtos.Propiedad
{
    public class PropiedadResponse
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public TipoPropiedadResponse TipoPropiedad { get; set; }
        public TipoVentaResponse TipoVenta { get; set; }
        public decimal Precio { get; set; }
        public decimal Terreno { get; set; }
        public int Habitaciones { get; set; }
        public int Banios { get; set; }
        public string Descripcion { get; set; }
        public List<MejoraResponse> Mejoras { get; set; }
        public int AgenteId { get; set; }
        public string AgenteName { get; set; }
        public EstadoPropiedad Estado { get; set; }
    }
}
