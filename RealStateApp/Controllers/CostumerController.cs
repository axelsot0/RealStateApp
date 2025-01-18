using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Properties;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Web.Controllers
{
    [Authorize(Roles = "Cliente")]
    [Route("[controller]/[action]")]
    public class CostumerController : Controller
    {
        private readonly IPropertyService _propiedadService;
        private readonly IFavoriteService _favoriteService;
        private readonly IGenericRepository<Cliente> _clienteRepo;

        public CostumerController(IPropertyService propiedadService, IFavoriteService favoriteService, IGenericRepository<Cliente> clienteRepo)
        {
            _propiedadService = propiedadService;
            _favoriteService = favoriteService;
            _clienteRepo = clienteRepo;
        }



        [HttpGet]
        public async Task<IActionResult> Home()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var cliente = await _clienteRepo.GetQuery().FirstOrDefaultAsync(c => c.UserId == userId);
            if (cliente == null)
            {
                ModelState.AddModelError(string.Empty, "Cliente no encontrado.");
                return View(new List<PropiedadViewModel>());
            }

            try
            {
                // Obtener todas las propiedades con la información relacionada.
                var propiedades = await _propiedadService.GetAllWithIncludeAsync();

                // Obtener la lista de propiedades favoritas del cliente.
                var favoritos = await _favoriteService.GetFavoritesByClienteIdAsync(cliente.Id);

                // Crear una lista para almacenar las propiedades favoritas.
                List<PropiedadViewModel> propiedadesFavoritas = new List<PropiedadViewModel>();

                if (favoritos != null && favoritos.Any())
                {
                    foreach (var fav in favoritos)
                    {
                        // Buscar la propiedad que coincida con el ID de la propiedad favorita.
                        
                        var propiedadFavorita = propiedades.FirstOrDefault(p => p.Id == fav.Id); // Cambié `PropiedadId` por `Id`
                        if (propiedadFavorita != null)
                        {
                            // Añadir la propiedad a la lista de propiedades favoritas.
                            propiedadFavorita.Imagenes = fav.Imagenes;
                            propiedadesFavoritas.Add(propiedadFavorita);
                        }
                    }

                    // Devolver la vista con las propiedades favoritas del cliente.
                    return View(propiedadesFavoritas);
                }
                else
                {
                    // Si no hay favoritos, devolver una lista vacía y un mensaje.
                    ModelState.AddModelError(string.Empty, "No tienes propiedades favoritas actualmente.");
                    return View(new List<PropiedadViewModel>()); // Retorna lista vacía
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(new List<PropiedadViewModel>());
            }
        }








        // Listado de agentes
        [HttpGet]
        public IActionResult Agents()
        {
            return RedirectToAction("Index", "Agent");
        }

        // Marcar una propiedad como favorita
        [HttpPost]
        public async Task<IActionResult> AddFavorite(int propiedadId)
        {
            var clienteId = int.Parse(User.FindFirst("UserId").Value);
            await _favoriteService.AddFavoriteAsync(clienteId, propiedadId);
            return RedirectToAction("Home");
        }

        // Desmarcar una propiedad como favorita
        [HttpPost]
        public async Task<IActionResult> RemoveFavorite(int propiedadId)
        {
            var user = HttpContext.User;
            if (!user.Identity.IsAuthenticated || !user.IsInRole("Cliente"))
            {
                return Unauthorized();
            }

            

            var userId = User.FindFirst("UserId")?.Value;
            var cliente = await _clienteRepo.GetQuery().FirstOrDefaultAsync(c => c.UserId == userId); 
            await _favoriteService.RemoveFavoriteAsync(cliente.Id, propiedadId);
            return RedirectToAction("Home");
        }

        // Cerrar sesión
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
