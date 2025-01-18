using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Dtos.Agente;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IAccountServiceWebApi : IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task ChangeAgenteActivation(int id, AgenteChangeStatus status);
        Task<string> GetAgenteName(int agenteId);
        Task<RegisterResponse> RegisterDeveloperAsync(RegisterDesarrolladorRequest request);
    }
}
