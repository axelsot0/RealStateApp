using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Interfaces.Services;
using System.Security.Claims;
using RealStateApp.Core.Application.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using RealStateApp.Infraestructure.Identity.Entities;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infraestructure.Persistence.Repositories;
using RealStateApp.Core.Application.Interfaces.Repositories;

namespace RealStateApp.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountServiceWebApp _accountService;
        private readonly IAgentService _agentService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepository<Cliente> _clienteRepo;

        public AccountController(IAccountServiceWebApp accountService, IAgentService agentService, UserManager<AppUser> userManager, IGenericRepository<Cliente> clienteRepo)
        {
            _accountService = accountService;
            _agentService = agentService;
            _userManager = userManager;
            _clienteRepo = clienteRepo;
        }




        
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loginResponse = await _accountService.LoginAsync(model);

            if (loginResponse == null || loginResponse.HasError)
            {
                
                ModelState.AddModelError(string.Empty, loginResponse?.ErrorMessage ?? "Ocurrió un error inesperado.");
                return View(model);
            }

            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginResponse.FirstName),
                new Claim(ClaimTypes.Email, loginResponse.Email),
                new Claim(ClaimTypes.Role, loginResponse.Role),
                new Claim("UserId", loginResponse.Id) 
            };

            
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe  
            };

            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);


            
            
            return loginResponse.Role switch
            {
                var role when role == Roles.Cliente.ToString() => RedirectToAction("Home", "Costumer"),
                var role when role == Roles.Agente.ToString() => RedirectToAction("Index", "Home"),
                var role when role == Roles.Admin.ToString() => RedirectToAction("Dashboard", "Admin"),
                _ => RedirectToAction("Index", "Home")
            };

        }

        
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Login");
        }


       
        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel();
            return View(model);
        }

        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                string profilePhotoUrl = null;

                if (model.Photo != null)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    Directory.CreateDirectory(uploadsFolder); 

                    var uniqueFileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Photo.CopyToAsync(stream);
                    }

                    profilePhotoUrl = $"/uploads/{uniqueFileName}"; 
                }

                
                var baseUrl = $"{Request.Scheme}://{Request.Host}"; 

               
                var result = await _accountService.RegisterUserAsync(model, baseUrl,profilePhotoUrl);

                if (result.HasError)
                {
                    ModelState.AddModelError("", result.Error);
                    return View(model);
                }
                
                
                if (model.Role == Roles.Agente.ToString())
                {
                    var addAgentDto = new AddAgentDto
                    {
                        ProfilePhotoURL = profilePhotoUrl,
                        UserId = result.Id 
                    };

                    await _agentService.AddAgent(addAgentDto);
                }

                
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ocurrió un error inesperado: {ex.Message}");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ActivateUser(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] = "Parámetros de activación no válidos.";
                return RedirectToAction("Index", "Home");
            }

            var result = await ConfirmUserEmailAsync(userId, token);

            if (result)
            {
                TempData["SuccessMessage"] = "Tu cuenta ha sido activada con éxito.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al activar la cuenta. El enlace puede haber expirado o ser inválido.";
            }

            return View(); 
        }


        
        public async Task<bool> ConfirmUserEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }
    }
}
