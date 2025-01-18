using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Mejoras;
using RealStateApp.Core.Application.Features.Mejoras.Commands.CreateMejora;
using RealStateApp.Core.Application.Features.Mejoras.Commands.DeleteMejoraById;
using RealStateApp.Core.Application.Features.Mejoras.Commands.UpdateMejora;
using RealStateApp.Core.Application.Features.Mejoras.Queries.GetAllMejoras;
using RealStateApp.Core.Application.Features.Mejoras.Queries.GetMejoraById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealStateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Mantenimiento de Mejoras")]
    public class MejoraController : BaseApiController
    {
        [Authorize(Roles = "Admin, Desarrollador")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MejoraResponse>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listado de mejoras",
            Description = "Obtiene todas las mejoras registradas en el sistema"
        )]
        public async Task<IActionResult> Get()
        {
            var mejoras = await Mediator.Send(new GetAllMejorasQuery());

            if(mejoras == null)
            {
                return NoContent();
            }

            return Ok(mejoras);
        }

        [Authorize(Roles = "Admin, Desarrollador")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MejoraResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Mejora por Id",
            Description = "Obtiene una mejora filtrado por su Id"
        )]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetMejoraByIdQuery() { Id = id }));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Creacion de mejora",
            Description = "Recibe los parametros necesarios para crear una nueva mejora"
        )]
        public async Task<IActionResult> Post([FromBody] CreateMejoraCommand command)
        {
            var response = await Mediator.Send(command);

            return CreatedAtAction(nameof(Get), new { response.Data }, response.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MejoraUpdateResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Actualizacion de mejora",
            Description = "Recibe los parametros necesarios para modificar una mejora existente"
        )]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateMejoraCommand command)
        {
            if (id != command.Id) return BadRequest("The Ids don't match");

            return Ok(await Mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Eliminar una mejora",
            Description = "Recibe los parametros necesarios para eliminar una mejora existente"
        )]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await Mediator.Send(new DeleteMejoraByIdCommand() { Id = id });

            return NoContent();
        }
    }
}
