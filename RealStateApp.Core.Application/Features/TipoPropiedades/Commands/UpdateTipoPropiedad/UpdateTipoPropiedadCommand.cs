using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Wrappers;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealStateApp.Core.Application.Features.TipoPropiedades.Commands.UpdateTipoPropiedad
{
    /// <summary>
    /// Parametros para la actualizacion de un tipo de propiedad
    /// </summary>
    public class UpdateTipoPropiedadCommand : IRequest<Response<TipoPropiedadUpdateResponse>>
    {
        [SwaggerParameter(Description = "Id del tipo de propiedad que se esta actualizando")]
        public int? Id { get; set; }

        [SwaggerParameter(Description = "Nuevo nombre del tipo de propiedad")]
        public string? Nombre { get; set; }

        [SwaggerParameter(Description = "Nueva descripcion del tipo de propiedad")]
        public string? Descripcion { get; set; }
    }

    public class UpdateTipoPropiedadCommadHandler : IRequestHandler<UpdateTipoPropiedadCommand, Response<TipoPropiedadUpdateResponse>>
    {
        private readonly ITipoPropiedadRepository _tipoPropiedadRepo;
        private readonly IMapper _mapper;

        public UpdateTipoPropiedadCommadHandler(ITipoPropiedadRepository tipoPropiedadRepo, IMapper mapper)
        {
            _tipoPropiedadRepo = tipoPropiedadRepo;
            _mapper = mapper;
        }

        public async Task<Response<TipoPropiedadUpdateResponse>> Handle(UpdateTipoPropiedadCommand command, CancellationToken cancellationToken)
        {
            var tipoPropiedad = await _tipoPropiedadRepo.GetByIdAsync(command.Id.Value);

            if (tipoPropiedad == null) throw new ApiException($"No existe tipo de propiedad con el Id {command.Id}", (int)HttpStatusCode.BadRequest);

            tipoPropiedad = _mapper.Map<TipoPropiedad>(command);

            await _tipoPropiedadRepo.UpdateAsync(tipoPropiedad, tipoPropiedad.Id);

            var data = _mapper.Map<TipoPropiedadUpdateResponse>(tipoPropiedad);

            return new Response<TipoPropiedadUpdateResponse>(data);
        }
    }
}
