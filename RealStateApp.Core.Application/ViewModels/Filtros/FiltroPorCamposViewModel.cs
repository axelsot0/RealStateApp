using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.Filtros
{
    public class FiltroPorCamposViewModel
    {
        public string TipoPropiedad { get; set; }
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }
        public int? Habitaciones { get; set; }
        public int? Banios { get; set; }
    }
}
