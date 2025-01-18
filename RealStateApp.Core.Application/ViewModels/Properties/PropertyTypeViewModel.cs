using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.Properties
{
    public class PropertyTypeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Introduzca un nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Agregue una descripcion")]
        public string Descripcion { get; set; }

        public int? Count { get; set; }
    }
}
