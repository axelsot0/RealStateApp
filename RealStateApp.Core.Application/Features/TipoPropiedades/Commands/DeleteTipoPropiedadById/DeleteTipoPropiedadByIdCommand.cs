using MediatR;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealStateApp.Core.Application.Features.TipoPropiedades.Commands.DeleteTipoPropiedadById
{
    /// <summary>
    /// Paramentros para la eliminacion de un tipo de propiedad
    /// </summary>
    public class DeleteTipoPropiedadByIdCommand : IRequest<Response<bool>>
    {
        [SwaggerParameter(Description = "Id del tipo de propiedad que se desea eliminar")]
        public int Id { get; set; }
    }

    public class DeleteTipoPropiedadByIdCommandHandler : IRequestHandler<DeleteTipoPropiedadByIdCommand, Response<bool>>
    {
        private readonly ITipoPropiedadRepository _tipoPropiedadRepo;

        public DeleteTipoPropiedadByIdCommandHandler(ITipoPropiedadRepository tipoPropiedadRepo)
        {
            _tipoPropiedadRepo = tipoPropiedadRepo;
        }

        public async Task<Response<bool>> Handle(DeleteTipoPropiedadByIdCommand command, CancellationToken cancellationToken)
        {
            var tipoPropiedad = await _tipoPropiedadRepo.GetByIdAsync(command.Id);

            if (tipoPropiedad == null) throw new ApiException($"No existe tipo de propiedad con en Id {command.Id}", (int)HttpStatusCode.BadRequest);

            await _tipoPropiedadRepo.DeleteAsync(tipoPropiedad);

            return new Response<bool>(true);
        }
    }
}
