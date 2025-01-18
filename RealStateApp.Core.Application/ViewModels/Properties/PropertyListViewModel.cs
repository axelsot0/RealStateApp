namespace RealStateApp.Core.Application.ViewModels.Properties
{
    public class PropertyListViewModel
    {
        public int Id { get; set; } // ID de la propiedad
        public string NombrePropiedad { get; set; } // Nombre o título de la propiedad
        public string ImagenUrl { get; set; } // URL de la imagen de la propiedad
        public decimal Precio { get; set; } // Precio de la propiedad
        public string TipoPropiedad { get; set; } // Tipo de propiedad (ej. Casa, Apartamento)
        public int Habitaciones { get; set; } // Cantidad de habitaciones
        public int Banos { get; set; } // Cantidad de baños
        public bool EsFavorita { get; set; } // Indica si es favorita
    }
}
