using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.Dtos.Account
{
    /// <summary>
    /// Parametros para realizar la autenticacion de usuario
    /// </summary>
    public class AuthenticationRequest
    {
        [SwaggerParameter(Description = "Correo del usuario que desea iniciar sesion")]
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [SwaggerParameter(Description = "Contrasenia del usuario que desea iniciar sesion")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
