using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Dtos.Agente;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Settings;
using RealStateApp.Infraestructure.Identity.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RealStateApp.Infraestructure.Identity.Services
{
    public class AccountServiceWebApi : BaseAccountService, IAccountServiceWebApi
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JWTSettings _jwtSettings;
        private readonly IAdminRepository _adminRepo;
        private readonly IAgenteRepository _agenteRepo;
        private readonly IDesarrolladorRepository _desarrolladorRepo;

        public AccountServiceWebApi(UserManager<AppUser> userManager,
                                    IOptions<JWTSettings> jwtSettings,
                                    IAdminRepository adminRepo,
                                    IAgenteRepository agenteRepo,
                                    IDesarrolladorRepository desarrolladorRepo) : base(userManager, adminRepo)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _desarrolladorRepo = desarrolladorRepo;
            _adminRepo = adminRepo;
            _agenteRepo = agenteRepo;
            _desarrolladorRepo = desarrolladorRepo;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse respond = new();

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                respond.HasError = true;
                respond.Error = $"No Accounts registered with {request.Email}";
                return respond;
            }

            if (!user.EmailConfirmed)
            {
                respond.HasError = true;
                respond.Error = $"Account no confirmed for {request.Email}";
                return respond;
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                respond.HasError = true;
                respond.Error = $"Invalid credential for {request.Email}";
                return respond;
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWT(user);

            respond.Id = user.Id;
            respond.Username = user.UserName;
            respond.Email = user.Email;

            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            respond.Roles = roles.ToList();
            respond.IsVerified = user.EmailConfirmed;
            respond.JWT = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            var refreshToken = GenerateRefreshToken();
            respond.RefreshToken = refreshToken.Token;

            return respond;
        }

        public async Task<RegisterResponse> RegisterDeveloperAsync(RegisterDesarrolladorRequest request)
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

            await _userManager.AddToRoleAsync(user, Roles.Desarrollador.ToString());

            var desarrollador = new Desarrollador
            {
                UserId = user.Id,
                Cedula = request.Cedula,
            };

            await _desarrolladorRepo.AddAsync(desarrollador);

            return respond;
        }

        public async Task ChangeAgenteActivation(int id, AgenteChangeStatus status)
        {
            var agente = await _agenteRepo.GetByIdAsync(id);

            if (agente == null) throw new Exception($"El agente con el Id {id} no existe");

            var account = await _userManager.FindByIdAsync(agente.UserId);

            account.EmailConfirmed = status.IsActive;

            var result = await _userManager.UpdateAsync(account);

            if (!result.Succeeded) throw new Exception("Ocurrio un error al cambiar el status del agente");
        }

        public async Task<string> GetAgenteName(int agenteId)
        {
            var agente = await _agenteRepo.GetByIdAsync(agenteId);

            if (agente == null) throw new Exception($"El agente con el Id {agenteId} no existe");

            var account = await _userManager.FindByIdAsync(agente.UserId);

            return $"{account.FirstName} {account.LastName}";
        }

        #region Private Methods
        private async Task<JwtSecurityToken> GenerateJWT(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken
            (
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
        }

        private string RandomTokenString()
        {
            var randomBytes = new byte[40];

            RandomNumberGenerator.Fill(randomBytes);

            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
        #endregion
    }
}
