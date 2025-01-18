using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.TipoPropiedades;
using RealStateApp.Core.Application.Features.TipoPropiedades.Commands.CreateTipoPropiedad;
using RealStateApp.Core.Application.Features.TipoPropiedades.Commands.DeleteTipoPropiedadById;
using RealStateApp.Core.Application.Features.TipoPropiedades.Commands.UpdateTipoPropiedad;
using RealStateApp.Core.Application.Features.TipoPropiedades.Queries.GetAllTipoPropiedades;
using RealStateApp.Core.Application.Features.TipoPropiedades.Queries.GetTipoPropiedadById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealStateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Mantenimiento de Tipos de Propiedades")]
    public class TipoPropiedadController : BaseApiController
    {
        [Authorize(Roles = "Admin, Desarrollador")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TipoPropiedadResponse>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listado de Tipos de propiedades",
            Description = "Obtiene todos los tipos de propiedades registrados en el sistema"
        )]
        public async Task<IActionResult> Get()
        {
            var tiposPropiedades = await Mediator.Send(new GetAllTipoPropiedadesQuery());

            if(tiposPropiedades == null)
            {
                return NoContent();
            }

            return Ok(tiposPropiedades);
        }

        [Authorize(Roles = "Admin, Desarrollador")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoPropiedadResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Tipo de propiedad por Id",
            Description = "Obtiene un tipo de propiedad filtrado por su Id"
        )]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            return Ok(await Mediator.Send(new GetTipoPropiedadByIdQuery() { Id = id }));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Creacion de Tipo de propiedad",
            Description = "Recibe los parametros necesarios para crear un nuevo tipo de propiedad"
        )]
        public async Task<IActionResult> Post([FromBody] CreateTipoPropiedadCommand command)
        {
            var response = await Mediator.Send(command);

            return CreatedAtAction(nameof(Get), new { response.Data }, response.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoPropiedadUpdateResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Actualizacion de tipo de propiedad",
            Description = "Recibe los parametros necesarios para modificar un tipo de propiedad existente"
        )]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateTipoPropiedadCommand command)
        {
            if (id != command.Id) return BadRequest("The Ids don't match");

            return Ok(await Mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Eliminar un tipo de propiedad",
            Description = "Recibe los parametros necesarios para eliminar un tipo de propiedad existente"
        )]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await Mediator.Send(new DeleteTipoPropiedadByIdCommand() { Id = id });

            return NoContent();
        }
    }
}
