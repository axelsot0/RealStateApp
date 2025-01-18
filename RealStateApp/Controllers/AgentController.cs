using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels.Agents;
using RealStateApp.Core.Application.ViewModels.Filtros;
using System.Security.Claims;


[Route("[controller]/[action]")]
public class AgentController : Controller
{

    private readonly IAgentService _agentService;
    private readonly IPropertyService _propertyService;

    public AgentController(IAgentService agentService, IPropertyService property)
    {
        _agentService = agentService;
        _propertyService = property;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string name = null)
    {
        
        var filter = new AgentFilter
        {
            Name = name 
        };

       
        var agents = await _agentService.GetAgents(filter);

        
        ViewData["CurrentFilter"] = name;

        return View(agents);
    }
    

    [HttpGet]
    public async Task<IActionResult> AgentProperties(int agentId)
    {
        
        var properties = await _propertyService.GetPropertiesByAgentIdAsync(agentId);

        
        if (properties == null || !properties.Any())
        {
            ViewData["Message"] = "No se encontraron propiedades para este agente.";
        }

        return View(properties);
    }

    [HttpGet]
    public async Task<IActionResult> MyProfile()
    {
        var userId = User.FindFirst("UserId")?.Value;
        var model = await _agentService.GetAgentProfileAsync(userId);
        return View(model);
    }
    [Authorize(Roles = "Agente")]

    [HttpPost]
    public async Task<IActionResult> MyProfile(EditAgentViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = User.FindFirst("UserId")?.Value;
        await _agentService.UpdateAgentProfileAsync(userId, model);

        return RedirectToAction("Index", "Home");
    }
    [Authorize(Roles = "Agente")]

    [HttpPost]
    public async Task<IActionResult> EditProfile(EditAgentViewModel model)
    {

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var userId = User.FindFirst("UserId")?.Value;
            await _agentService.UpdateAgentProfileAsync(userId, model);

            TempData["Success"] = "El perfil del agente se actualizó correctamente.";
            return RedirectToAction("Index", "Agent");
        }
        catch (ApplicationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado.");
            return View(model);
        }
    }


}
