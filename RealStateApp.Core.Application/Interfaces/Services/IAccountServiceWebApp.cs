using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.ViewModels.User.Agente;
using RealStateApp.Core.Application.ViewModels.Agents;
using RealStateApp.Core.Application.ViewModels.Filtros;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Application.ViewModels.User.Admin;
using RealStateApp.Core.Application.ViewModels.View;
using RealStateApp.Core.Application.ViewModels.User.Desarrollador;


namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IAccountServiceWebApp : IAccountService
    {

        Task<bool> UpdateUserState(string id);

        Task<List<AppAdminViewModel>> GetAllAdmins();

        Task<List<AppAgentViewModel>> GetAllAgents();

        Task<SystemOverviewViewModel> SystemOverview();

        Task<bool> UserTypeDelete(string id);

        Task<RegisterResponse> UserTypeCreate(CreateUserViewModel model);

        Task<RegisterResponse> RegisterUserAsync(RegisterViewModel registerViewModel, string baseUrl, string profilePhotoUrl);
        Task<List<AgentViewModel>> GetAllAgents(AgentFilter filter);
        
        Task<LoginResponse> LoginAsync(LoginViewModel loginViewModel);

        Task<AppAgentViewModel> GetAgentById(string id);

        Task UpdateAgentAsync(string agentId, EditAgentViewModel updatedAgent);

        Task<AppDeveloperViewModel> GetDeveloperById(string id);

        Task<List<AppDeveloperViewModel>> GetAllDevelopers();

        Task<DeveloperHandlerViewModel> GetDeveloperHanlerById(string id);

        Task<bool> EditDeveloper(DeveloperHandlerViewModel model);

        Task<AdminHandlerViewModel> GetAdminHanlerById(string id);

        Task<bool> EditAdmin(AdminHandlerViewModel model);
    }
}