using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.Dtos.Agente
{
    /// <summary>
    /// Parametros para activar o inactivar un agente
    /// </summary>
    public class AgenteChangeStatus
    {
        [SwaggerParameter(Description = "Indica si el agente esta activo (true) o inactivo (false)")]
        [Required(ErrorMessage = "IsActive is required")]
        public bool IsActive { get; set; }
    }
}
