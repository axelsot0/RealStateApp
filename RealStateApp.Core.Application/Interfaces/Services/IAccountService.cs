using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.ViewModels.User;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        
        Task<AccountDto> GetAccountByUserIdAsync(string userId);
        Task<RegisterResponse> RegisterAdminAsync(RegisterAdminRequest request);
        
    }
}
