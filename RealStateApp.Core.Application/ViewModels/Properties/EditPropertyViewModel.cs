using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.Properties
{
    public class EditPropertyViewModel
    {
        public int Id { get; set; }

        [Required]
        public int TipoPropiedadId { get; set; }

        [Required]
        public int TipoVentaId { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public decimal Precio { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        [Range(1, 99999999)]
        public decimal TamañoPropiedad { get; set; }

        [Required]
        public int Habitaciones { get; set; }

        [Required]
        public int Banios { get; set; }

        public List<int> MejoraIds { get; set; } = new List<int>();
        public List<IFormFile> Imagenes { get; set; } = new List<IFormFile>();
        public List<string> ImagenesExistentes { get; set; } = new List<string>();

    }
}
