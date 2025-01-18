namespace RealStateApp.Core.Application.ViewModels.Favorites
{
    public class FavoritePropertyViewModel
    {
        public int ClienteId { get; set; } // ID del cliente
        public int PropiedadId { get; set; } // ID de la propiedad
        public string NombrePropiedad { get; set; } // Nombre o título de la propiedad
        public string ImagenUrl { get; set; } // URL de la imagen de la propiedad
        public decimal Precio { get; set; } // Precio de la propiedad
        
        public bool EsFavorita { get; set; } // Indica si es favorita
    }
}
