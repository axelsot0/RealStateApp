using Microsoft.AspNetCore.Identity;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infraestructure.Identity.Entities;
using System.Diagnostics;

namespace RealStateApp.Infraestructure.Identity.Services
{
    public abstract class BaseAccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAdminRepository _adminRepo;

        protected BaseAccountService(UserManager<AppUser> userManager, IAdminRepository adminRepo)
        {
            _userManager = userManager;
            _adminRepo = adminRepo;
        }

        public async Task<RegisterResponse> RegisterAdminAsync(RegisterAdminRequest request)
        {
            RegisterResponse respond = new();

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userWithSameEmail != null)
            {
                respond.HasError = true;
                respond.Error = $"{request.Email} is already registered";
                return respond;
            }

            var userWithSameUsername = await _userManager.FindByNameAsync(request.Username);

            if (userWithSameUsername != null)
            {
                respond.HasError = true;
                respond.Error = $"{request.Username} is already registered";
                return respond;
            }

            var user = new AppUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Username,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                respond.HasError = true;
                respond.Error = $"An error occurred trying to register the user";
                return respond;
            }

            await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());

            var admin = new Admin
            {
                UserId = user.Id,
                Cedula = request.Cedula,
            };

            await _adminRepo.AddAsync(admin);

            return respond;
        }

        public async Task<AccountDto> GetAccountByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                Debug.WriteLine("El userId proporcionado es nulo o vacío.");
                return null;
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                Debug.WriteLine($"No se encontró un usuario con el userId: {userId}");
                return null;
            }

            return new AccountDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,

            };
        }

    }
}
