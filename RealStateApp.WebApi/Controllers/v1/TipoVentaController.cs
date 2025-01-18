using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.TipoVentas;
using RealStateApp.Core.Application.Features.TipoVentas.Commands.CreateTipoVenta;
using RealStateApp.Core.Application.Features.TipoVentas.Commands.DeleteTipoVentaById;
using RealStateApp.Core.Application.Features.TipoVentas.Commands.UpdateTipoVenta;
using RealStateApp.Core.Application.Features.TipoVentas.Queries.GetAllTipoVentas;
using RealStateApp.Core.Application.Features.TipoVentas.Queries.GetTipoVentaById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealStateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Mantenimiento de Tipo de Ventas")]
    public class TipoVentaController : BaseApiController
    {
        [Authorize(Roles = "Admin, Desarrollador")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TipoVentaResponse>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listado de Tipos de ventas",
            Description = "Obtiene todos los tipos de ventas registrados en el sistema"
        )]
        public async Task<IActionResult> Get()
        {
            var tiposVenta = await Mediator.Send(new GetAllTipoVentasQuery());

            if (tiposVenta == null)
            {
                return NoContent();
            }

            return Ok(tiposVenta);
        }

        [Authorize(Roles = "Admin, Desarrollador")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoVentaResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Tipo de venta por Id",
            Description = "Obtiene un tipo de venta filtrado por su Id"
        )]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetTipoVentaByIdQuery() { Id = id }));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Creacion de Tipo de venta",
            Description = "Recibe los parametros necesarios para crear un nuevo tipo de venta"
        )]
        public async Task<IActionResult> Post([FromBody] CreateTipoVentaCommand command)
        {
            var response = await Mediator.Send(command);

            return CreatedAtAction(nameof(Get), new { response.Data }, response.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoVentaUpdateResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Actualizacion de tipo de venta",
            Description = "Recibe los parametros necesarios para modificar un tipo de venta existente"
        )]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateTipoVentaCommand command)
        {
            if (id != command.Id) return BadRequest("The Ids don't match");

            return Ok(await Mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Eliminar un tipo de venta",
            Description = "Recibe los parametros necesarios para eliminar un tipo de venta existente"
        )]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await Mediator.Send(new DeleteTipoVentaByIdCommand() { Id = id });

            return NoContent();
        }
    }
}
