using MediatR;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealStateApp.Core.Application.Features.Mejoras.Commands.DeleteMejoraById
{
    /// <summary>
    /// Paramentros para la eliminacion de una mejora
    /// </summary>
    public class DeleteMejoraByIdCommand : IRequest<Response<bool>>
    {
        [SwaggerParameter(Description = "Id de la mejora que se desea eliminar")]
        public int Id { get; set; }
    }

    public class DeleteMejoraByIdCommandHandler : IRequestHandler<DeleteMejoraByIdCommand, Response<bool>>
    {
        private readonly IMejoraRepository _mejoraRepo;

        public DeleteMejoraByIdCommandHandler(IMejoraRepository mejoraRepo)
        {
            _mejoraRepo = mejoraRepo;
        }

        public async Task<Response<bool>> Handle(DeleteMejoraByIdCommand command, CancellationToken cancellationToken)
        {
            var mejora = await _mejoraRepo.GetByIdAsync(command.Id);

            if (mejora == null) throw new ApiException($"No existe mejora con el Id {command.Id}", (int)HttpStatusCode.BadRequest);

            await _mejoraRepo.DeleteAsync(mejora);

            return new Response<bool>(true);
        }
    }
}
