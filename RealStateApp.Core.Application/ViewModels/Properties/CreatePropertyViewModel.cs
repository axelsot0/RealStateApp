using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace RealStateApp.Core.Application.ViewModels.Properties
{
    public class CreatePropertyViewModel
    {
        [Required(ErrorMessage = "El tipo de propiedad es obligatorio.")]
        public int TipoPropiedadId { get; set; }

        [Required(ErrorMessage = "El tipo de venta es obligatorio.")]
        public int TipoVentaId { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El tamaño de la propiedad es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El tamaño debe ser mayor que cero.")]
        public double TamañoPropiedad { get; set; }

        [Required(ErrorMessage = "La cantidad de habitaciones es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe haber al menos una habitación.")]
        public int Habitaciones { get; set; }

        [Required(ErrorMessage = "La cantidad de baños es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe haber al menos un baño.")]
        public int Banios { get; set; }

        [Required(ErrorMessage = "Debe seleccionar al menos una mejora.")]
        public List<int> MejoraIds { get; set; }

        [Required(ErrorMessage = "Debe subir al menos una imagen.")]
        [MaxLength(4, ErrorMessage = "Puede subir hasta 4 imágenes.")]
        public List<IFormFile>? Imagenes { get; set; }
    }
}

