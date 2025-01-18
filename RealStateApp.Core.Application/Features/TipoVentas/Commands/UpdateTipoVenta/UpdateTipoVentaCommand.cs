using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Wrappers;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealStateApp.Core.Application.Features.TipoVentas.Commands.UpdateTipoVenta
{
    /// <summary>
    /// Parametros para la actualizacion de un tipo de venta
    /// </summary>
    public class UpdateTipoVentaCommand : IRequest<Response<TipoVentaUpdateResponse>>
    {
        [SwaggerParameter(Description = "Id del tipo de venta que se esta actualizando")]
        public int? Id { get; set; }

        [SwaggerParameter(Description = "Nuevo nombre del tipo de venta")]
        public string? Nombre { get; set; }

        [SwaggerParameter(Description = "Nueva descripcion del tipo de venta")]
        public string? Descripcion { get; set; }
    }

    public class UpdateTipoVentaCommandHandler : IRequestHandler<UpdateTipoVentaCommand, Response<TipoVentaUpdateResponse>>
    {
        private readonly ITipoVentaRepository _tipoVentaRepo;
        private readonly IMapper _mapper;

        public UpdateTipoVentaCommandHandler(ITipoVentaRepository tipoVentaRepo, IMapper mapper)
        {
            _tipoVentaRepo = tipoVentaRepo;
            _mapper = mapper;
        }

        public async Task<Response<TipoVentaUpdateResponse>> Handle(UpdateTipoVentaCommand command, CancellationToken cancellationToken)
        {
            var tipoVenta = await _tipoVentaRepo.GetByIdAsync(command.Id.Value);

            if (tipoVenta == null) throw new ApiException($"No existe tipo de venta con el Id {command.Id}", (int)HttpStatusCode.BadRequest);

            tipoVenta = _mapper.Map<TipoVenta>(command);

            await _tipoVentaRepo.UpdateAsync(tipoVenta, tipoVenta.Id);

            var data = _mapper.Map<TipoVentaUpdateResponse>(tipoVenta);

            return new Response<TipoVentaUpdateResponse>(data);
        }
    }
}
