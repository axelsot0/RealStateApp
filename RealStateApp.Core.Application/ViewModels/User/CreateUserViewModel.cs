using RealStateApp.Core.Application.Dtos.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.User
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage ="No puede dejar su nombre vacío")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "No puede dejar su apellido vacío")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Introduzca un nombre de usuario")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Debe colocar su correo")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La cedula es requerida")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d{3}-?\d{7}-?\d{1}$", ErrorMessage = "El Cedula debe tener el formato XXX-XXXXXXX-X")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "Debe crear un contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Debe confirmar su contraseña")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }
    }
}
