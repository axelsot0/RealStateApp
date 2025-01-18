using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.Dtos.Account
{
    /// <summary>
    /// Parametro para registrar un usuario desarrollador
    /// </summary>
    public class RegisterDesarrolladorRequest
    {
        [SwaggerParameter("Nombre del usuario que sea registrarse")]
        [Required(ErrorMessage = "FirstName is required")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [SwaggerParameter("Apellido del usuario que sea registrarse")]
        [Required(ErrorMessage = "LastName is required")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [SwaggerParameter("Cedula del usuario que sea registrarse")]
        [Required(ErrorMessage = "Cedula is required")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d{3}-?\d{7}-?\d{1}$", ErrorMessage = "El Cedula debe tener el formato XXX-XXXXXXX-X")]
        public string Cedula { get; set; }

        [SwaggerParameter("Correo del usuario que sea registrarse")]
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [SwaggerParameter("Nombre de usuario del usuario que sea registrarse")]
        [Required(ErrorMessage = "Username is required")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [SwaggerParameter("Contrasenia del usuario que sea registrarse")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [SwaggerParameter("Confirmacion de la contrasenia del usuario que sea registrarse")]
        [Required(ErrorMessage = "ConfirmPassword is required")]
        [Compare(nameof(Password), ErrorMessage = "Password and ConfirmPassword don't match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
