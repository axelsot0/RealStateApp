using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Properties;
using RealStateApp.Core.Application.ViewModels.Filtros;
using RealStateApp.Core.Application.ViewModels.View;
using RealStateApp.Core.Domain.Entities;
using System.Security.Claims;
using RealStateApp.Core.Application.Enums;

namespace RealStateApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IFavoriteService _favoriteService;
        private readonly IGenericRepository<Cliente> _clienteRepo;
        private readonly IGenericRepository<Agente> _agenteRepo;

        public HomeController(IPropertyService propertyService, IFavoriteService favoriteService, IGenericRepository<Cliente> clienteRepo, IGenericRepository<Agente> agenteRepo)
        {
            _propertyService = propertyService;
            _favoriteService = favoriteService;
            _clienteRepo = clienteRepo;
            _agenteRepo = agenteRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index(HomeFilterViewModel filtros)
        {
            var user = HttpContext.User;
            ICollection<PropiedadViewModel> propiedades = new List<PropiedadViewModel>();
            string userRole = "";
            
            propiedades = await _propertyService.FiltroPropiedad(filtros, 0);
            if (user.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst("UserId")?.Value;

                if (user.IsInRole("Cliente"))
                {
                    var cliente = await _clienteRepo.GetQuery().FirstOrDefaultAsync(c => c.UserId == userId);
                    userRole = Roles.Cliente.ToString();

                    if (cliente != null)
                    {
                        propiedades = await _propertyService.FiltroPropiedad(filtros, cliente.Id);
                    }
                }

                else if (user.IsInRole("Agente"))
                {
                    var agente = await _agenteRepo.GetQuery().FirstOrDefaultAsync(a => a.UserId == userId);
                    userRole = Roles.Agente.ToString();

                    if (agente != null)
                    {
                        propiedades = await _propertyService.GetPropertiesByAgentIdAsync(agente.Id);
                    }
                }
            }
            

            var tiposPropiedades = await _propertyService.GetTiposPropiedadAsync();

            var homeViewModel = new HomeViewModel
            {
                Filtros = filtros,
                Propiedades = propiedades,
                TiposPropiedades = tiposPropiedades,
                UserRole = userRole
            };

            return View(homeViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> AddToFavorites(int propiedadId)
        {
            var user = HttpContext.User;

            if (!user.Identity.IsAuthenticated || !user.IsInRole("Cliente"))
            {
                return Unauthorized();
            }
            var userId = User.FindFirst("UserId")?.Value;
            
            var cliente = await _clienteRepo.GetQuery().FirstOrDefaultAsync(c => c.UserId == userId);

            if (cliente == null)
            {
                return BadRequest("Cliente no encontrado.");
            }

            await _favoriteService.AddFavoriteAsync(cliente.Id, propiedadId);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromFavorites(int propiedadId)
        {
            var user = HttpContext.User;

            if (!user.Identity.IsAuthenticated || !user.IsInRole("Cliente"))
            {
                return Unauthorized();
            }

            var userId = User.FindFirst("UserId")?.Value;
            var cliente = await _clienteRepo.GetQuery().FirstOrDefaultAsync(c => c.UserId == userId);

            if (cliente == null)
            {
                return BadRequest("Cliente no encontrado.");
            }

            await _favoriteService.RemoveFavoriteAsync(cliente.Id, propiedadId);

            return RedirectToAction(nameof(Index));
        }
    }
}
