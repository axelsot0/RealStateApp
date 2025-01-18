namespace RealStateApp.Core.Application.ViewModels.Favorites
{
    public class FavoriteListViewModel
    {
        public int ClienteId { get; set; } // Id del Cliente
        public string ClienteNombre { get; set; } // Nombre del Cliente
        public List<FavoritePropertyViewModel> Favoritos { get; set; } // Lista de propiedades favoritas

        public FavoriteListViewModel()
        {
            Favoritos = new List<FavoritePropertyViewModel>();
        }
    }
}
