using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.TipoPropiedades;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealStateApp.Core.Application.Features.TipoPropiedades.Queries.GetTipoPropiedadById
{
    /// <summary>
    /// Parametros para obtener un tipo de propiedad por su Id
    /// </summary>
    public class GetTipoPropiedadByIdQuery : IRequest<Response<TipoPropiedadResponse>>
    {
        [SwaggerParameter(Description = "Colocar Id del tipo de propiedad que desea obtener")]
        public int Id { get; set; }
    }

    public class GetTipoPropiedadByIdQueryHandler : IRequestHandler<GetTipoPropiedadByIdQuery, Response<TipoPropiedadResponse>>
    {
        private readonly ITipoPropiedadRepository _tipoPropiedadRepo;
        private readonly IMapper _mapper;

        public GetTipoPropiedadByIdQueryHandler(ITipoPropiedadRepository tipoPropiedadRepo, IMapper mapper)
        {
            _tipoPropiedadRepo = tipoPropiedadRepo;
            _mapper = mapper;
        }

        public async Task<Response<TipoPropiedadResponse>> Handle(GetTipoPropiedadByIdQuery request, CancellationToken cancellationToken)
        {
            var tipoPropiedad = await _tipoPropiedadRepo.GetByIdAsync(request.Id);

            if (tipoPropiedad == null) throw new ApiException($"No existe tipo de propiedad con el Id {request.Id}", (int)HttpStatusCode.BadRequest);

            var data = _mapper.Map<TipoPropiedadResponse>(tipoPropiedad);

            return new Response<TipoPropiedadResponse>(data);
        }
    }
}
