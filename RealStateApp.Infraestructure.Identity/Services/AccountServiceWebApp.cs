using Microsoft.AspNetCore.Identity;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Infraestructure.Identity.Entities;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.ViewModels.User.Admin;

using RealStateApp.Core.Application.ViewModels.View;
using RealStateApp.Core.Application.ViewModels.User.Agente;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity.Data;
using RealStateApp.Core.Application.Dtos.Account;

using RealStateApp.Core.Application.ViewModels.Agents;
using RealStateApp.Core.Application.ViewModels.Filtros;
using RealStateApp.Core.Application.Dtos.Email;
using RealStateApp.Core.Application.ViewModels.User.Desarrollador;
using System.Reflection.Metadata.Ecma335;


namespace RealStateApp.Infraestructure.Identity.Services
{
    public class AccountServiceWebApp : BaseAccountService, IAccountServiceWebApp
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IAdminRepository _adminRepo;
        private readonly IGenericRepository<Cliente> _clientRepo;
        private readonly IPropiedadRepository _propertyRepo;
        
        private readonly IAgenteRepository _agentRepo;
        private readonly IDesarrolladorRepository _devRepo;

        public AccountServiceWebApp(UserManager<AppUser> userManager,
                                    SignInManager<AppUser> signInManager,
                                    IPropiedadRepository propiedadRepo,
                                    IClientRepository clientRepo,
                                    IAgenteRepository agentRepo,
                                    IDesarrolladorRepository devRepo,
                                    IAdminRepository adminRepo,
                                    IEmailService emailService
                                    ) : base(userManager, adminRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _propertyRepo = propiedadRepo;
            _clientRepo = clientRepo;
            _agentRepo = agentRepo;
            _devRepo = devRepo;
            _adminRepo = adminRepo;
            _emailService = emailService;
        }

        #region Admin
            public async Task<List<AppAdminViewModel>> GetAllAdmins()
            {
                var identityUsers = await _userManager.GetUsersInRoleAsync(Roles.Admin.ToString());
                var adminsFromDBO = await _adminRepo.GetAllAsync();

                var list = (from admin in adminsFromDBO
                            join identityUser in identityUsers 
                            on admin.UserId equals identityUser.Id
                            select new AppAdminViewModel
                            {
                                Id = identityUser.Id,
                                FirstName = identityUser.FirstName,
                                EmailConfirmed = identityUser.EmailConfirmed,
                                LastName = identityUser.LastName,
                                UserName = identityUser.UserName,
                                Email = identityUser.Email,
                                Cedula = admin.Cedula
                            }).ToList();

                return list;
            }

            public async Task<bool> EditAdmin(AdminHandlerViewModel model)
            {

                try
                {
                    AppUser user = new();
                    user = await _userManager.FindByIdAsync(model.Id);
                    
                    if (user != null) 
                    {
                        user.UserName = model.UserName;
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.Email = model.Email;

                        if (model.Password != null) 
                        { 
                            var removeResult = await _userManager.RemovePasswordAsync(user);
                            if (removeResult.Succeeded)
                            {
                            
                                var addResult = await _userManager.AddPasswordAsync(user, model.Password);
                                if (addResult.Succeeded)
                                {
                            
                                }
                                else
                                {
                                    foreach (var error in addResult.Errors)
                                    {
                                        Console.WriteLine(error.Description);
                                    }
                                }
                            }

                            await _userManager.UpdateAsync(user);

                            var admin = await _adminRepo.GetByIdAsync(model.AdminId);

                            admin.Cedula = model.Cedula;

                            await _adminRepo.UpdateAsync(admin, admin.Id);

                            return true;
                        }
                        
                        return true;
                    }
                    else{ return false;}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
            }

            public async Task<AdminHandlerViewModel> GetAdminHanlerById(string id)
            {
                var identityUsers = await _userManager.GetUsersInRoleAsync(Roles.Admin.ToString());
                var adminsFromDBO = await _adminRepo.GetAllAsync();

                var result = (from admins in adminsFromDBO
                                   join identityUser in identityUsers
                                   on admins.UserId equals identityUser.Id
                                   where identityUser.Id == id
                                   select new AdminHandlerViewModel
                                   {
                                       Id = identityUser.Id,
                                       UserName = identityUser.UserName,
                                       FirstName = identityUser.FirstName,
                                       LastName = identityUser.LastName,
                                       Email = identityUser.Email,
                                       EmailConfirmed = identityUser.EmailConfirmed,
                                       AdminId = admins.Id,
                                       Cedula = admins.Cedula                                    
                                   }).FirstOrDefault();

                return result;
            }
        #endregion

        #region Desarrollador
            public async Task<AppDeveloperViewModel> GetDeveloperById(string id)
            {
                var identityUsers = await _userManager.GetUsersInRoleAsync(Roles.Desarrollador.ToString());
                var devsFromDBO = await _devRepo.GetAllAsync();

                var result = (from developer in devsFromDBO
                                   join identityUser in identityUsers
                                   on developer.UserId equals identityUser.Id
                                   where identityUser.Id == id
                                   select new AppDeveloperViewModel
                                   {
                                       Id = identityUser.Id,
                                       FirstName = identityUser.FirstName,
                                       LastName = identityUser.LastName,
                                       Email = identityUser.Email,
                                       EmailConfirmed = identityUser.EmailConfirmed,
                                       Cedula = developer.Cedula

                                   }).FirstOrDefault();

                return result;
            }

            public async Task<DeveloperHandlerViewModel> GetDeveloperHanlerById(string id)
            {
                var identityUsers = await _userManager.GetUsersInRoleAsync(Roles.Desarrollador.ToString());
                var devsFromDBO = await _devRepo.GetAllAsync();

                var result = (from developer in devsFromDBO
                                   join identityUser in identityUsers
                                   on developer.UserId equals identityUser.Id
                                   where identityUser.Id == id
                                   select new DeveloperHandlerViewModel
                                   {
                                       Id = identityUser.Id,
                                       UserName = identityUser.UserName,
                                       FirstName = identityUser.FirstName,
                                       LastName = identityUser.LastName,
                                       Email = identityUser.Email,
                                       EmailConfirmed = identityUser.EmailConfirmed,
                                       DevId = developer.Id,
                                       Cedula = developer.Cedula                                    
                                   }).FirstOrDefault();

                return result;
            }

            public async Task<List<AppDeveloperViewModel>> GetAllDevelopers()
            {
                var identityUsers = await _userManager.GetUsersInRoleAsync(Roles.Desarrollador.ToString());
                var devsFromDBO = await _devRepo.GetAllAsync();

                var list = (from devs in devsFromDBO
                            join identityUser in identityUsers
                            on devs.UserId equals identityUser.Id
                            select new AppDeveloperViewModel
                            {
                                Id = identityUser.Id,
                                FirstName = identityUser.FirstName,
                                EmailConfirmed = identityUser.EmailConfirmed,
                                LastName = identityUser.LastName,
                                UserName = identityUser.UserName,
                                Email = identityUser.Email,
                                Cedula = devs.Cedula
                            }).ToList();

                return list;
            }

            public async Task<bool> EditDeveloper(DeveloperHandlerViewModel model)
            {
                
                try
                {
                    AppUser user = new(); 
                    user = await _userManager.FindByIdAsync(model.Id);
                
                    user.UserName = model.UserName;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    
                    if (user != null)
                    {
                        if (model.Password != null) 
                            { 
                                var removeResult = await _userManager.RemovePasswordAsync(user);
                                if (removeResult.Succeeded)
                                {
                            
                                    var addResult = await _userManager.AddPasswordAsync(user, model.Password);
                                    if (addResult.Succeeded)
                                    {
                            
                                    }
                                    else
                                    {
                                        foreach (var error in addResult.Errors)
                                        {
                                            Console.WriteLine(error.Description);
                                        }
                                    }
                                }

                                await _userManager.UpdateAsync(user);

                                var dev = await _devRepo.GetByIdAsync(model.DevId);

                                dev.Cedula = model.Cedula;

                                await _devRepo.UpdateAsync(dev, dev.Id);

                                return true;
                        }   
                    
                        return true;    
                    }

                    return false;
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return false; 
                }
            }
        #endregion

        #region Agentes
            public async Task<List<AppAgentViewModel>> GetAllAgents()
            {
                var identityUsers = await _userManager.GetUsersInRoleAsync(Roles.Agente.ToString());
                var agentsFromDBO = await _agentRepo.GetAllAsync();
                var properties = await _propertyRepo.GetAllAsync();

                var list = (from agent in agentsFromDBO
                            join identityUser in identityUsers
                            on agent.UserId equals identityUser.Id
                            select new AppAgentViewModel
                            {
                                Id = identityUser.Id,
                                UserName = identityUser.UserName,
                                Firstname = identityUser.FirstName,
                                Lastname = identityUser.LastName,
                                Email = identityUser.Email,
                                EmailConfirmed = identityUser.EmailConfirmed,
                                PropertyCount = properties.Count(p => p.AgenteId == agent.Id),
                            }).ToList();
                return list;
            }

            public async Task<List<AgentViewModel>> GetAllAgents(AgentFilter filter)
            {
                var identityUsers = await _userManager.GetUsersInRoleAsync(Roles.Agente.ToString());

            
                identityUsers = identityUsers.Where(user => user.EmailConfirmed).ToList();

                var agentsFromDBO = await _agentRepo.GetAllAsync();


            if (!string.IsNullOrEmpty(filter.Name))
            {
                identityUsers = identityUsers.Where(user => $"{user.FirstName} {user.LastName}"
                    .StartsWith(filter.Name, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var list = (from agent in agentsFromDBO
                            join identityUser in identityUsers on agent.UserId equals identityUser.Id
                            select new AgentViewModel
                            {
                                Id = agent.Id,
                                Name = $"{identityUser.FirstName} {identityUser.LastName}",
                                ProfilePhotoUrl = agent.ProfilePhotoURL,
                                UserId = identityUser.Id

                            }).ToList();

                return list;
            }

            public async Task<AppAgentViewModel> GetAgentById(string id)
            {
                var identityUsers = await _userManager.GetUsersInRoleAsync(Roles.Agente.ToString());
                var agentsFromDBO = await _agentRepo.GetAllAsync();
                var properties = await _propertyRepo.GetAllAsync();

                var result = (from agent in agentsFromDBO
                                    join identityUser in identityUsers
                                    on agent.UserId equals identityUser.Id
                                    where identityUser.Id == id
                                    select new AppAgentViewModel
                                    {
                                        Id = identityUser.Id,
                                        Firstname = identityUser.FirstName,
                                        Lastname = identityUser.LastName,
                                        Email = identityUser.Email,
                                        EmailConfirmed = identityUser.EmailConfirmed,
                                        PropertyCount = (from agents in agentsFromDBO
                                                        join properties in properties
                                                        on agents.Id equals properties.AgenteId
                                                        where agents.UserId == id
                                                        select agents).Count()
                                    }).FirstOrDefault();

                return result;
            }
        public async Task UpdateAgentAsync(string agentId, EditAgentViewModel editAgent)
        {
            try
            {
                
                var user = await _userManager.FindByIdAsync(agentId);

                
                if (user == null)
                {
                    throw new KeyNotFoundException($"No se encontró el usuario con el ID {agentId}.");
                }

                
                user.FirstName = editAgent.Nombre;
                user.LastName = editAgent.Apellido;
                user.PhoneNumber = editAgent.Telefono;

                

               
                var result = await _userManager.UpdateAsync(user);

                
                if (!result.Succeeded)
                {
                    var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new ApplicationException($"Error al actualizar el usuario: {errorMessage}");
                }
            }
            catch (KeyNotFoundException ex)
            {
                
                throw new ApplicationException($"Error: {ex.Message}", ex);
            }
            catch (ApplicationException ex)
            {
                
                throw new ApplicationException($"Error de aplicación: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException($"Ocurrió un error inesperado al actualizar el agente: {ex.Message}", ex);
            }
        }

        #endregion

        #region Misc
        public async Task<RegisterResponse> UserTypeCreate(CreateUserViewModel model)
            {
                var respond = new RegisterResponse();

                try
                {
                    var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);

                    if (userWithSameEmail != null)
                    {
                        respond.HasError = true;
                        respond.Error = $"{model.Email} is already registered";
                        return respond;
                    }

                    var userWithSameUsername = await _userManager.FindByNameAsync(model.Username);

                    if (userWithSameUsername != null)
                    {
                        respond.HasError = true;
                        respond.Error = $"{model.Username} is already registered";
                        return respond;
                    }

                    AppUser user = new AppUser 
                    { 
                        UserName = model.Username,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        EmailConfirmed = true,
                    };


                        var result = await _userManager.CreateAsync(user, model.Password);

                        if (!result.Succeeded)
                        {
                            foreach(var obj in result.Errors)
                            {
                                Console.WriteLine(obj.Description.ToString());
                            }
                        }

                    var createdUser = await _userManager.FindByEmailAsync(model.Email);

                    if (createdUser != null)
                    {
                        switch (model.Role)
                        {
                            case "Admin":
                                await _userManager.AddToRoleAsync(createdUser, Roles.Admin.ToString());
                                break;

                            case "Developer":
                                await _userManager.AddToRoleAsync(createdUser, Roles.Desarrollador.ToString());
                                break;
                        }
                    }

                    
                    var roles = await _userManager.GetRolesAsync(createdUser);

                    if (roles == null)
                    {
                        respond.HasError = true;
                        respond.Error = "No se encontraron roles en este usuario";

                        return respond;
                    }

                    if (roles.Contains(Roles.Admin.ToString()))
                    {
                        var admin = new Admin
                        {
                            UserId = createdUser.Id,
                            Cedula = model.Cedula
                        };

                        await _adminRepo.AddAsync(admin);
                    }

                    if (roles.Contains(Roles.Desarrollador.ToString()))
                    {
                        var dev = new Desarrollador
                        {
                            UserId = createdUser.Id,
                            Cedula = model.Cedula
                        };

                        await _devRepo.AddAsync(dev);
                    }

                    return respond;
                }
                catch (Exception ex)
                {
                    respond.HasError = true;
                    respond.Error = ex.Message;

                    return respond;
                }
            }

            public async Task<bool> UserTypeDelete(string id)
            {
                try
                {
                    var deletedUser = await _userManager.FindByIdAsync(id);

                    if (deletedUser == null)
                    {
                        return false;
                    }

                    var roles = await _userManager.GetRolesAsync(deletedUser);

                    if (roles == null || roles.Count == 0)
                    {
                        Console.WriteLine("El usuario no tiene roles.");
                        return false;
                    }

                    foreach (var role in roles)
                    {
                        Console.WriteLine($"Usuario {deletedUser.UserName} tiene el rol: {role}");
                    }


                    if (roles.Contains(Roles.Agente.ToString()))
                    {
                        var agent = await _agentRepo.GetQuery().FirstOrDefaultAsync(agt => agt.UserId == deletedUser.Id);
                        await _agentRepo.DeleteAsync(agent);
                    }

                    if (roles.Contains(Roles.Desarrollador.ToString()))
                    {
                        var dev = await _devRepo.GetQuery().FirstOrDefaultAsync(dev => dev.UserId == deletedUser.Id);
                        await _devRepo.DeleteAsync(dev);
                    }

                    await _userManager.DeleteAsync(deletedUser);

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            public async Task<SystemOverviewViewModel> SystemOverview()
            {
                var IdentityClientUsers = await _userManager.GetUsersInRoleAsync(Roles.Cliente.ToString());
                var IdentityAgentUsers = await _userManager.GetUsersInRoleAsync(Roles.Agente.ToString());
                var IdentityDeveloperUsers = await _userManager.GetUsersInRoleAsync(Roles.Desarrollador.ToString());

                var clients = await _clientRepo.GetAllAsync();
                var agents = await _agentRepo.GetAllAsync();
                var developers = await _devRepo.GetAllAsync();

                var propiedades = await _propertyRepo.GetAllAsync();
                var pcount = propiedades.Count();

                var ActiveAgentCount = (from agente in agents
                                        join identityAgentUser in IdentityAgentUsers
                                        on agente.UserId equals identityAgentUser.Id
                                        where identityAgentUser.EmailConfirmed = true
                                        select agente).Count();

                var InactiveAgentCount = (from agente in agents
                                          join identityAgentUser in IdentityAgentUsers
                                          on agente.UserId equals identityAgentUser.Id
                                          where identityAgentUser.EmailConfirmed = false
                                          select agente).Count();

                var ActiveClientCount = (from cliente in clients
                                         join identityClientUser in IdentityClientUsers
                                         on cliente.UserId equals identityClientUser.Id
                                         where identityClientUser.EmailConfirmed = true
                                         select cliente).Count();

                var InactiveClientCount = (from cliente in clients
                                           join identityClientUser in IdentityClientUsers
                                           on cliente.UserId equals identityClientUser.Id
                                           where identityClientUser.EmailConfirmed = false
                                           select cliente).Count();

                var ActiveDevCount = (from devs in developers
                                      join identityDeveloperUser in IdentityDeveloperUsers
                                      on devs.UserId equals identityDeveloperUser.Id
                                      where identityDeveloperUser.EmailConfirmed = true
                                      select devs).Count();

                var InactiveDevCount = (from devs in developers
                                        join identityDeveloperUser in IdentityDeveloperUsers
                                        on devs.UserId equals identityDeveloperUser.Id
                                        where identityDeveloperUser.EmailConfirmed = false
                                        select devs).Count();


                var model = new SystemOverviewViewModel
                {
                    PropertyCount = pcount,
                    ActiveAgentsCount = ActiveAgentCount,
                    InactiveAgentsCount = InactiveAgentCount,
                    ActiveClientsCount = ActiveClientCount,
                    InactiveClientsCount = InactiveClientCount,
                    ActiveDevelopersCount = ActiveDevCount,
                    InactiveDevelopersCount = InactiveDevCount
                };

                return model;
            }

            public async Task<bool> UpdateUserState(string id)
            {
                try
                {
                    
                    var user = await _userManager.FindByIdAsync(id);

                    
                    if (user == null)
                    {
                        return false;
                    }

                    Console.WriteLine(user.EmailConfirmed);

                    var bol = !user.EmailConfirmed;

                    user.EmailConfirmed = bol;
                    Console.WriteLine(user.EmailConfirmed);

                    await _userManager.UpdateAsync(user);

                }
                catch (Exception ex)
                {
                    return false;
                }

                return true;
            }
        #endregion

        #region Login and registration
        public async Task<LoginResponse> LoginAsync(LoginViewModel loginViewModel)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(loginViewModel.EmailOrUsername);

                    if (loginViewModel.EmailOrUsername.Contains("@"))
                    {
                        user = await _userManager.FindByEmailAsync(loginViewModel.EmailOrUsername);
                    }

                    if (user == null)
                    {
                        return new LoginResponse { HasError = true, ErrorMessage = "El correo no está registrado." };
                    }

                    if (!await _userManager.CheckPasswordAsync(user, loginViewModel.Password))
                    {
                        return new LoginResponse { HasError = true, ErrorMessage = "La contraseña es incorrecta." };
                    }

                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        return new LoginResponse { HasError = true, ErrorMessage = "Email no verificado." };
                    }

                    var roles = await _userManager.GetRolesAsync(user);

                    if (!roles.Any())
                    {
                        return new LoginResponse { HasError = true, ErrorMessage = "El usuario no tiene un rol asignado." };
                    }

                    return new LoginResponse
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Role = roles.FirstOrDefault(),
                        HasError = false
                    };
                }
                catch (Exception ex)
                {
                    return new LoginResponse { HasError = true, ErrorMessage = "Ocurrió un error inesperado." };
                }
            }

            public async Task LogoutAsync()
            {
                await _signInManager.SignOutAsync();
            }

        public async Task<RegisterResponse> RegisterUserAsync(RegisterViewModel registerViewModel, string baseUrl, string profilePhotoUrl)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(registerViewModel.Email);
                if (existingUser != null)
                {
                    return new RegisterResponse { HasError = true, Error = "El correo ya está registrado." };
                }

                var user = new AppUser
                {
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName,
                    UserName = registerViewModel.UserName,
                    Email = registerViewModel.Email,
                    EmailConfirmed = false
                };

                var result = await _userManager.CreateAsync(user, registerViewModel.Password);
                if (!result.Succeeded)
                {
                    return new RegisterResponse { HasError = true, Error = "No se pudo registrar el usuario." };
                }

                var role = registerViewModel.Role == "Agente" ? Roles.Agente.ToString() : Roles.Cliente.ToString();
                await _userManager.AddToRoleAsync(user, role);

                if (registerViewModel.Role == "Cliente")
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var activationLink = $"{baseUrl}/Account/ActivateUser?userId={user.Id}&token={Uri.EscapeDataString(token)}";

                    var emailRequest = new EmailRequest
                    {
                        To = user.Email,
                        Subject = "Activa tu cuenta",
                        Body = $"Activa tu cuenta haciendo clic en <a href='{activationLink}'>este enlace</a>."
                    };
                    var userId= (await _userManager.FindByEmailAsync(registerViewModel.Email)).Id;
                    Cliente cliente = new Cliente
                    {
                        UserId = userId,
                        ProfilePhotoURL = profilePhotoUrl 
                    };

                    
                    await _clientRepo.AddAsync(cliente);

                    await _emailService.SendAsync(emailRequest);
                }

                return new RegisterResponse { Id = user.Id, HasError = false };
            }
            catch (Exception ex)
            {
                return new RegisterResponse { HasError = true, Error = "Ocurrió un error al registrar el usuario." };
            }
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
        #endregion
    }
}