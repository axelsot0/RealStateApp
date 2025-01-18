namespace RealStateApp.Core.Domain.Entities
{
    public class PropiedadCliente
    {
        public int ClienteId { get; set; }
        public int PropiedadId { get; set; }
        public Cliente Cliente { get; set; }
        public Propiedad Propiedad { get; set; }
    }
}
