using System.Collections.Generic;
using RealStateApp.Core.Application.ViewModels.Filtros;
using RealStateApp.Core.Application.ViewModels.Properties;

namespace RealStateApp.Core.Application.ViewModels.View
{
    public class HomeViewModel
    {
        public HomeFilterViewModel Filtros { get; set; } 
        public ICollection<PropiedadViewModel> Propiedades { get; set; } 
        public List<PropertyTypeViewModel> TiposPropiedades { get; set; } 
        public string UserRole { get; set; } 

        public HomeViewModel()
        {
            
            Filtros = new HomeFilterViewModel();
            Propiedades = new List<PropiedadViewModel>();
            TiposPropiedades = new List<PropertyTypeViewModel>();
        }
    }
}
