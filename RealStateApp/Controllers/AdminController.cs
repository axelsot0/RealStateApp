using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Properties;
using RealStateApp.Core.Application.ViewModels.Sales;
using RealStateApp.Core.Application.ViewModels.Upgrades;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Application.ViewModels.User.Admin;
using RealStateApp.Core.Application.ViewModels.User.Agente;
using RealStateApp.Core.Application.ViewModels.User.Desarrollador;
using System.Security.Claims;

namespace RealStateApp.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly IAccountServiceWebApp _accountService;
        private readonly IPropertyTypeService _propertyService;
        private readonly ISaleTypeService _saleService;
        private readonly IMejoraService _mejoraService;

        public AdminController(IAccountServiceWebApp accountService, IPropertyTypeService propertyService, ISaleTypeService saleService, 
            IMejoraService mejoraService)
        {
            _accountService = accountService;
            _propertyService = propertyService;
            _saleService = saleService;
            _mejoraService = mejoraService;
        }

        #region Admin
            public async Task<IActionResult> Dashboard()
            {
                try
                {
                    var lista = await _accountService.SystemOverview();

                    return View(lista);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            public async Task<IActionResult> Admin()
            {
                var userId = User.FindFirstValue("UserId");
                var lista = await _accountService.GetAllAdmins();

                var currentUser = lista.FirstOrDefault(u => u.Id == userId);

                if (currentUser != null)
                {
                    lista.Remove(currentUser);
                }

                return View(lista);
            }

            public IActionResult ChangeAdminS(string Id)
            {
                AppAdminViewModel model = new();
                model.Id = Id;

                return View(model);
            }

            [HttpPost]
            public async Task<IActionResult> Admin(AppAdminViewModel model)
            {
                //var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _accountService.UpdateUserState(model.Id);
                
                if(result)
                {
                    var lista = await _accountService.GetAllAdmins();
                    return RedirectToAction("Admin", lista);
                }
                else
                {
                    return View(model);
                }
            }

            public async Task<IActionResult> EditAdmin(string Id)
            {                
                var model = await _accountService.GetAdminHanlerById(Id);

                return View(model);
            }

            [HttpPost]
            public async Task<IActionResult> EditAdminPost(AdminHandlerViewModel model)
            {
                try
                {
                    if (model.Password != null || model.ConfirmPassword != null) 
                    { 
                        var validPass = model.Password.Equals(model.ConfirmPassword);
                        
                        if (!validPass) 
                        {
                            return RedirectToAction("EditAdmin", model.Id);
                        }
                    }
                    
                    var result = await _accountService.EditAdmin(model);

                    if (result) 
                    { 
                        return RedirectToAction("Admin", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("EditAdmin", model.Id);
                    }
                }
                catch
                {
                    return RedirectToAction("EditAdmin", model.Id);
                }
            }
        #endregion

        #region Agent
            public async Task<IActionResult> Agent()
                {
                    var lista = await _accountService.GetAllAgents();
                    
                    return View(lista);
                }

            public IActionResult ChangeAgentS(string Id)
            {
                AppAgentViewModel model = new();
                model.Id = Id;

                return View(model);
            }

            [HttpPost]
            public async Task<IActionResult> AgentPost(AppAgentViewModel model)
            {
                //var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _accountService.UpdateUserState(model.Id);
                
                if(result)
                {
                    var lista = await _accountService.GetAllAgents();
                    return RedirectToAction("Agent", lista);
                }
                else
                {
                    return RedirectToAction("Agent", model);
                }
            }

            #region Delete
                public async Task<IActionResult> DeleteAgent(string Id)
                {
                    var model = await _accountService.GetAgentById(Id);

                    return View(model);
                }

                [HttpPost]
                public async Task<IActionResult> DeleteAgentPost(string Id)
                {
                    try
                    {
                        Console.WriteLine(Id);
                        var result = await _accountService.UserTypeDelete(Id);

                        return RedirectToAction("Agent", "Admin");
                    }
                    catch (Exception ex)
                    {
                        var model = await _accountService.GetAgentById(Id);

                        return View("DeleteAgent", model);
                    }
                }
            #endregion

        #endregion

        #region Developer
            public async Task<IActionResult> Developer()
            {
                var lista = await _accountService.GetAllDevelopers();

                return View(lista);
            }

            public IActionResult ChangeDeveloperS(string Id)
            {
                AppDeveloperViewModel model = new();
                model.Id = Id;

                return View(model);
            }

            [HttpPost]
            public async Task<IActionResult> Developer(AppDeveloperViewModel model)
            {
                //var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _accountService.UpdateUserState(model.Id);
                
                if(result)
                {
                    var lista = await _accountService.GetAllDevelopers();
                    return RedirectToAction("Developer", lista);
                }
                else
                {
                    return View(model);
                }
            }

            public async Task<IActionResult> EditDeveloper(string Id)
            {
                var model = await _accountService.GetDeveloperHanlerById(Id);

                return View(model);
            }

            [HttpPost]
            public async Task<IActionResult> EditDevPost(DeveloperHandlerViewModel model)
            {
                try
                {
                    if (model.Password != null && model.ConfirmPassword != null) 
                    { 
                        bool validPass = model.Password == model.ConfirmPassword;
                        
                        if (!validPass) 
                        {
                            return RedirectToAction("EditDeveloper", model.Id);
                        }
                    }
                    
                    var result = await _accountService.EditDeveloper(model);

                    if (result) 
                    { 
                        return RedirectToAction("Developer", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("EditDeveloper", "Admin", model.Id);
                    }
                    
                }
                catch
                {
                    return RedirectToAction("EditDeveloper", "Admin",model.Id);
                }
            }
        #endregion

        #region Admin_and_dev

            [HttpPost]
            public IActionResult CreateAdmin(CreateUserViewModel model)
            {
                model.Role = "Admin";
                return RedirectToAction("Create", model);
            }

            [HttpPost]
            public IActionResult CreateDeveloper(CreateUserViewModel model)
            {   
                model.Role = "Developer";
                return RedirectToAction("Create", model);
            }

            public IActionResult Create(CreateUserViewModel model) 
            {
                return View(model);
            }

            [HttpPost]
            public async Task<IActionResult> CreatePost(CreateUserViewModel model)
            {
                if (model.Password != null && model.Password == model.ConfirmPassword) 
                {

                    var response = await _accountService.UserTypeCreate(model);

                    if (response.HasError)
                    {
                        return RedirectToAction("Create", model);
                    }

                    if (model.Role == "Admin")
                    {
                        return RedirectToAction("Admin", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Developer", "Admin");
                    }
                }
                else
                {
                    return RedirectToAction("Create", model);
                }
            }
        #endregion

        #region Property type

            public async Task<IActionResult> PropertyType()
            {
                var model = await _propertyService.GetTiposPropiedadAsync();

                return View(model);
            }

            public async Task<IActionResult> EditPropertyT(int Id)
            {
                var model = await _propertyService.GetPropertyTypeById(Id);

                return View(model);
            }

            [HttpPost]
            public async Task<IActionResult> EditPropertyTPost(PropertyTypeViewModel model)
            {
                try
                {   
                    await _propertyService.Update(model, model.Id);

                    return RedirectToAction("PropertyType", "Admin");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("EditPropertyT", model.Id);
                }
            }

            #region Delete
            public async Task<IActionResult> DeletePropertyT(int Id)
                    {
                        var model = await _propertyService.GetPropertyTypeById(Id);

                        return View(model);
                    }

                    [HttpPost]
                    public async Task<IActionResult> DeletePropertyTPost(int Id)
                    {
                        try
                        {
                            Console.WriteLine(Id);
                            var result = await _propertyService.DeletePropertyType(Id);

                            return RedirectToAction("PropertyType", "Admin");
                        }
                        catch (Exception ex)
                        {
                            var model = await _propertyService.GetPropertyTypeById(Id);

                            return View("DeletePropertyType", model);
                        }
                    }
                #endregion
            
            #region Create
                [HttpGet]
                public IActionResult CreatePropertyT(PropertyTypeViewModel model)
                {
                    return View(model);
                }

                [HttpPost]
                public async Task<IActionResult> CreatePropertyTPost(PropertyTypeViewModel model)
                {
                    var response = await _propertyService.CreatePropertyType(model);

                    if (response == false)
                    {
                        return RedirectToAction("CreatePropertyT", model);
                    }
                    else
                    {
                        return RedirectToAction("PropertyType", "Admin");
                    }
                }
            #endregion

        #endregion

        #region Sale type
            public async Task<IActionResult> SaleType()
            {
                var model = await _saleService.GetTiposVentaAsync();

                return View(model);
            }

            public async Task<IActionResult> EditSaleT(int Id)
            {
                var model = await _saleService.GetById(Id);

                return View(model);
            }

            [HttpPost]
            public async Task<IActionResult> EditSaleTPost(SaleTypeViewModel model)
            {
                try
                {
                    await _saleService.Update(model, model.Id);

                    return RedirectToAction("SaleType", "Admin");
                }
                catch
                {
                    return RedirectToAction("EditUpgrade", model.Id);
                }
            }

        #region Create
        [HttpGet]
                    public IActionResult CreateSaleT(SaleTypeHandlerViewModel model)
                    {
                        return View(model);
                    }

                    [HttpPost]
                    public async Task<IActionResult> CreateSaleTPost(SaleTypeHandlerViewModel model)
                    {
                        var response = await _saleService.Add(model);

                        if (response == null)
                        {
                            return RedirectToAction("CreateSaleT", model);
                        }
                        else
                        {
                            return RedirectToAction("SaleType", "Admin");
                        }
                    }
        #endregion

            #region Delete
                public async Task<IActionResult> DeleteSaleT(int Id)
                {
                    var model = await _saleService.GetById(Id);

                    return View(model);
                }

                [HttpPost]
                public async Task<IActionResult> DeleteSaleTPost(int Id)
                {
                    try
                    {
                        Console.WriteLine(Id);
                        await _saleService.Delete(Id);

                        return RedirectToAction("SaleType", "Admin");
                    }
                    catch (Exception ex)
                    {
                        var model = await _saleService.GetById(Id);

                        return View("DeleteSaleT", model);
                    }
                }
            #endregion

        #endregion

        #region Mejoras

            public async Task<IActionResult> Upgrades()
            {
                var model = await _mejoraService.GetAll();

                return View(model);
            }
            
            public async Task<IActionResult> EditUpgrade(int Id) 
            {
                var model = await _mejoraService.GetById(Id);

                return View(model);
            }

            [HttpPost]
            public async Task<IActionResult> EditUpgradePost(UpgradeViewModel model) 
            {    
                try
                {
                    await _mejoraService.Update(model, model.Id);
               
                    return RedirectToAction("Upgrades", "Admin");
                }
                catch
                {
                    return RedirectToAction("EditUpgrade", model.Id);
                }
            }
            
            #region Create

                    [HttpGet]
                    public IActionResult CreateUpgrade(UpgradeHandlerViewModel model)
                    {
                        return View(model);
                    }

                    [HttpPost]
                    public async Task<IActionResult> CreateUpgradePost(UpgradeHandlerViewModel model)
                    {
                        var response = await _mejoraService.Add(model);

                        if (response == null)
                        {
                            return RedirectToAction("CreateUpgrade", model);
                        }
                        else
                        {
                            return RedirectToAction("Upgrades", "Admin");
                        }
                    }

                #endregion

            #region Delete

                public async Task<IActionResult> DeleteUpgrade(int Id)
                {
                    var model = await _mejoraService.GetById(Id);

                    return View(model);
                }

                [HttpPost]
                public async Task<IActionResult> DeleteUpgradePost(int Id)
                {
                    try
                    {
                        Console.WriteLine(Id);
                        await _mejoraService.Delete(Id);

                        return RedirectToAction("Upgrades", "Admin");
                    }
                    catch (Exception ex)
                    {
                        var model = await _mejoraService.GetById(Id);

                        return View("DeleteUpgrade", model);
                    }
                }

            #endregion

        #endregion
    }
}