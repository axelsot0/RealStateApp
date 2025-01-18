namespace RealStateApp.Core.Application.ViewModels.Properties
{
    public class PropiedadViewModel
    {
        public int Id { get; set; }
        public string TipoPropiedad { get; set; }
        public List<string> Imagenes { get; set; } = new List<string>(); // Lista de URLs de imágenes
        public string CodigoPropiedad { get; set; }
        public string TipoVenta { get; set; }
        public decimal ValorPropiedad { get; set; }
        public int CantidadHabitaciones { get; set; }
        public int CantidadBaños { get; set; }
        public float TamañoPropiedad { get; set; } // Medido en metros cuadrados (Mt)
        public bool IsFavorite { get; set; }
    }
}
