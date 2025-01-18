using MediatR;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealStateApp.Core.Application.Features.TipoVentas.Commands.DeleteTipoVentaById
{
    /// <summary>
    /// Paramentros para la eliminacion de un tipo de venta
    /// </summary>
    public class DeleteTipoVentaByIdCommand : IRequest<Response<bool>>
    {
        [SwaggerParameter(Description = "Id del tipo de venta que se desea eliminar")]
        public int Id { get; set; }
    }

    public class DeleteTipoVentaByIdCommandHandler : IRequestHandler<DeleteTipoVentaByIdCommand, Response<bool>>
    {
        private readonly ITipoVentaRepository _tipoVentaRepo;

        public DeleteTipoVentaByIdCommandHandler(ITipoVentaRepository tipoVentaRepo)
        {
            _tipoVentaRepo = tipoVentaRepo;
        }

        public async Task<Response<bool>> Handle(DeleteTipoVentaByIdCommand command, CancellationToken cancellationToken)
        {
            var tipoVenta = await _tipoVentaRepo.GetByIdAsync(command.Id);

            if (tipoVenta == null) throw new ApiException($"No existe tipo de venta con en Id {command.Id}", (int)HttpStatusCode.BadRequest);

            await _tipoVentaRepo.DeleteAsync(tipoVenta);

            return new Response<bool>(true);
        }
    }
}
