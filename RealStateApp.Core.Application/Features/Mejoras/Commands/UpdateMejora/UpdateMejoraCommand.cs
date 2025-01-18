using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Wrappers;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealStateApp.Core.Application.Features.Mejoras.Commands.UpdateMejora
{
    
    public class UpdateMejoraCommand : IRequest<Response<MejoraUpdateResponse>>
    {
        [SwaggerParameter(Description = "Id de la mejora que se esta actualizando")]
        public int? Id { get; set; }

        [SwaggerParameter(Description = "Nuevo nombre de la mejora")]
        public string? Nombre { get; set; }

        [SwaggerParameter(Description = "Nueva descripcion de la mejora")]
        public string? Descripcion { get; set; }
    }

    public class UpdateMejoraCommandHandler : IRequestHandler<UpdateMejoraCommand, Response<MejoraUpdateResponse>>
    {
        private readonly IMejoraRepository _mejoraRepo;
        private readonly IMapper _mapper;

        public UpdateMejoraCommandHandler(IMejoraRepository mejoraRepo, IMapper mapper)
        {
            _mejoraRepo = mejoraRepo;
            _mapper = mapper;
        }

        public async Task<Response<MejoraUpdateResponse>> Handle(UpdateMejoraCommand command, CancellationToken cancellationToken)
        {
            var mejora = await _mejoraRepo.GetByIdAsync(command.Id.Value);

            if (mejora == null) throw new ApiException($"No existe mejora con el Id {command.Id}", (int)HttpStatusCode.BadRequest);

            mejora = _mapper.Map<Mejora>(command);

            await _mejoraRepo.UpdateAsync(mejora, mejora.Id);

            var data = _mapper.Map<MejoraUpdateResponse>(mejora);

            return new Response<MejoraUpdateResponse>(data);
        }
    }
}
