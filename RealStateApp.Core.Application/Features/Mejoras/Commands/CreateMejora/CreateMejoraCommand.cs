using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Wrappers;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Core.Application.Features.Mejoras.Commands.CreateMejora
{
    /// <summary>
    /// Parametros para la creacion de una mejora
    /// </summary>
    public class CreateMejoraCommand : IRequest<Response<int>>
    {
        /// <example>Piscina</example>
        [SwaggerParameter(Description = "Nombre de la mejora")]
        public string? Nombre { get; set; }

        /// <example>Piscina rectangular de 10x5 metros con sistema de filtrado</example>
        [SwaggerParameter(Description = "Descripcion de la mejora")]
        public string? Descripcion { get; set; }
    }

    public class CreateMejoraCommandHandler : IRequestHandler<CreateMejoraCommand, Response<int>>
    {
        private readonly IMejoraRepository _mejoraRepo;
        private readonly IMapper _mapper;

        public CreateMejoraCommandHandler(IMejoraRepository mejoraRepo, IMapper mapper)
        {
            _mejoraRepo = mejoraRepo;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateMejoraCommand command, CancellationToken cancellationToken)
        {
            Mejora mejora = _mapper.Map<Mejora>(command);

            mejora = await _mejoraRepo.AddAsync(mejora);

            return new Response<int>(mejora.Id);
        }
    }
}
