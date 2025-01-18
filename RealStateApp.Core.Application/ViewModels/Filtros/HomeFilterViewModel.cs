using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.Filtros
{
    public class HomeFilterViewModel
    {
        public FiltroPorCodigoViewModel FiltroCodigo { get; set; }
        public FiltroPorCamposViewModel FiltroCampos { get; set; }
        public HomeFilterViewModel()
        {
            FiltroCampos = new FiltroPorCamposViewModel(); 
        }
    }
}
