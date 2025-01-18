using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Propiedad;
using RealStateApp.Core.Application.Features.Propiedades.Queries.GetAllPropiedades;
using RealStateApp.Core.Application.Features.Propiedades.Queries.GetPropiedadByCodigo;
using RealStateApp.Core.Application.Features.Propiedades.Queries.GetPropiedadById;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Controlador para la gestión de propiedades, " +
        "incluyendo la obtención de todas las propiedades, búsqueda por ID y búsqueda por código.")]
    [Authorize(Roles = "Admin, Desarrollador")]
    public class PropiedadController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PropiedadResponse>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listado de propiedades",
            Description = "Obtiene todas las propiedades registradas en el sistema"
        )]
        public async Task<IActionResult> Get()
        {
            var propiedades = await Mediator.Send(new GetAllPropiedadesQuery());

            if(propiedades == null)
            {
                return NoContent();
            }

            return Ok(propiedades);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropiedadResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Propiedad por Id",
            Description = "Obtiene una propiedad filtrado por su Id"
        )]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetPropiedadByIdQuery() { Id = id }));
        }

        [HttpGet("GetByCode/{codigo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropiedadResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Propiedad por Codigo",
            Description = "Obtiene una propiedad filtrado por su codigo"
        )]
        public async Task<IActionResult> Get([FromRoute] string codigo)
        {
            return Ok(await Mediator.Send(new GetPropiedadByCodigoQuery() { Codigo = codigo }));
        }
    }
}
