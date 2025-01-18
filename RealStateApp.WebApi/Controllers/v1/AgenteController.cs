using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Agente;
using RealStateApp.Core.Application.Dtos.Propiedad;
using RealStateApp.Core.Application.Features.Agentes.Queries.GetAgenteById;
using RealStateApp.Core.Application.Features.Agentes.Queries.GetAllAgentes;
using RealStateApp.Core.Application.Features.Propiedades.Queries.GetAllPropiedadesByAgenteId;
using RealStateApp.Core.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Controlador para el manejo de agentes. " +
        "Incluye endpoints para obtener los detalles de un agente, sus propiedades y modificar su status")]
    public class AgenteController : BaseApiController
    {
        private readonly IAccountServiceWebApi _accountService;
       
        public AgenteController(IAccountServiceWebApi accountService)
        {
            _accountService = accountService;
        }

        [Authorize(Roles = "Admin, Desarrollador")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AgenteResponse>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listado de agentes",
            Description = "Obtiene todos los agentes registrados en el sistema"
        )]
        public async Task<IActionResult> Get()
        {
            var agentes = await Mediator.Send(new GetAllAgentesQuery());

            if(agentes == null)
            {
                return NoContent();
            }

            return Ok(agentes);
        }

        [Authorize(Roles = "Admin, Desarrollador")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgenteResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Agente por Id",
            Description = "Obtiene un agente filtrado por su Id"
        )]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetAgenteByIdQuery() { Id = id }));
        }

        [Authorize(Roles = "Admin, Desarrollador")]
        [HttpGet("GetPropiedadesByAgenteId/{agenteId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PropiedadResponse>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listado de propiedades de un agente",
            Description = "Obtiene todas las propiedades de un agente filtrado por su Id"
        )]
        public async Task<IActionResult> GetPropiedadesByAgenteId([FromRoute] int agenteId)
        {
            var propiedades = await Mediator.Send(new GetAllPropiedadesByAgenteIdQuery() { AgenteId = agenteId });

            if(propiedades == null)
            {
                return NoContent();
            }

            return Ok(propiedades);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Activar o desactivar un agente",
            Description = "Permite a los administradores activar o desactivar un agente existente filtrado por su Id."            
        )]
        public async Task<IActionResult> ChangeStatus([FromRoute]int id, [FromBody]AgenteChangeStatus status)
        {
            await _accountService.ChangeAgenteActivation(id, status);

            return NoContent();
        }
    }
}
