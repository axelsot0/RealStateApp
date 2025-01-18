using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealStateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Controlador para la autenticacion de usuarios y registrar Administradores y Desarrolladores")]
    public class AccountController : BaseApiController
    {
        private readonly IAccountServiceWebApi _accountService;

        public AccountController(IAccountServiceWebApi accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("authenticate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Autentication de usuario",
            Description = "Permite autenticarse y obtener el JWT para poder usar las demás funcionalidades de la aplicación"
        )]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationRequest request)
        {
            if (User.Identity.IsAuthenticated)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You already are Authenticated");
            }

            var response = await _accountService.AuthenticateAsync(request);

            if (response.HasError)
            {
                return BadRequest(response.Error);
            }

            return Ok(response);
        }

        [HttpPost("register-developer")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Registro de usuario desarrollador",
            Description = "Permite crear usuario con el rol de desarrollador"
        )]
        public async Task<IActionResult> RegisterDeveloperAsync([FromBody] RegisterDesarrolladorRequest request)
        {
            var response = await _accountService.RegisterDeveloperAsync(request);

            if (response.HasError)
            {
                return BadRequest(response.Error);
            }

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register-admin")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Registro de usuario administrador",
            Description = "Permite crear usuarios con el rol de administrador. Debe haber un usuario logueado y ser de tipo administrador para poder usar este endpoint"
        )]
        public async Task<IActionResult> RegisterAdminAsync([FromBody] RegisterAdminRequest request)
        {
            var response = await _accountService.RegisterAdminAsync(request);

            if (response.HasError)
            {
                return BadRequest(response.Error);
            }

            return Ok(response);
        }
    }
}
