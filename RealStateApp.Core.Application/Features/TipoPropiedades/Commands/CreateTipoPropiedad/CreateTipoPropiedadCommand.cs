using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Wrappers;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Core.Application.Features.TipoPropiedades.Commands.CreateTipoPropiedad
{
    /// <summary>
    /// Parametros para la creacion de un tipo de propiedad
    /// </summary>
    public class CreateTipoPropiedadCommand : IRequest<Response<int>>
    {
        /// <example>Apartamento</example>
        [SwaggerParameter(Description = "Nombre del tipo de propiedad")]
        public string? Nombre { get; set; }

        /// <example>Apartamento con varias habitaciones, baños y espacios comunes. Ideal para vivir en áreas urbanas o residenciales.</example>
        [SwaggerParameter(Description = "Descripcion del tipo de propiedad")]
        public string? Descripcion { get; set; }
    }

    public class CreateTipoPropiedadCommandHandler : IRequestHandler<CreateTipoPropiedadCommand, Response<int>>
    {
        private readonly ITipoPropiedadRepository _tipoPropiedadRepo;
        private readonly IMapper _mapper;

        public CreateTipoPropiedadCommandHandler(ITipoPropiedadRepository tipoPropiedadRepo, IMapper mapper)
        {
            _tipoPropiedadRepo = tipoPropiedadRepo;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateTipoPropiedadCommand command, CancellationToken cancellationToken)
        {
            TipoPropiedad tipoPropiedad = _mapper.Map<TipoPropiedad>(command);
            tipoPropiedad = await _tipoPropiedadRepo.AddAsync(tipoPropiedad);
            return new Response<int>(tipoPropiedad.Id);
        }
    }
}
