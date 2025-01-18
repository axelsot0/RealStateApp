using System.ComponentModel.DataAnnotations;


namespace RealStateApp.Core.Application.ViewModels.User.Desarrollador
{
    public class DeveloperHandlerViewModel
    {
        [Required(ErrorMessage = "Falta el id")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Falta el username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Falta el nombre")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Falta el apellido")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Falta correo")]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        [Required(ErrorMessage = "Falta la cedula")]
        public string Cedula { get; set; }

        public string? Password { get; set; }

        public int DevId { get; set; }

        public string? ConfirmPassword { get; set; }
    }
}
