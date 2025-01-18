using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Propiedad;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealStateApp.Core.Application.Features.Propiedades.Queries.GetPropiedadByCodigo
{
    /// <summary>
    /// Parametros para obtener una propiedad por su codigo
    /// </summary>
    public class GetPropiedadByCodigoQuery : IRequest<Response<PropiedadResponse>>
    {
        [SwaggerParameter(Description = "Colocar codigo de la propiedad que desea obtener")]
        public string Codigo { get; set; }
    }

    public class GetPropiedadByCodigoQueryHandler : IRequestHandler<GetPropiedadByCodigoQuery, Response<PropiedadResponse>>
    {
        private readonly IPropiedadRepository _propiedadRepo;
        private readonly IAccountServiceWebApi _accountService;
        private readonly IMapper _mapper;

        public GetPropiedadByCodigoQueryHandler(IPropiedadRepository propiedadRepo, IAccountServiceWebApi accountService, IMapper mapper)
        {
            _propiedadRepo = propiedadRepo;
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<Response<PropiedadResponse>> Handle(GetPropiedadByCodigoQuery request, CancellationToken cancellationToken)
        {
            var propiedad = await _propiedadRepo.GetByCodeWithInclude(request.Codigo);

            if (propiedad == null) throw new ApiException($"La propiedad con el codigo {request.Codigo} no existe", (int)HttpStatusCode.BadRequest);

            var propiedadResponse = _mapper.Map<PropiedadResponse>(propiedad);

            propiedadResponse.AgenteName = await _accountService.GetAgenteName(propiedadResponse.AgenteId);

            return new Response<PropiedadResponse>(propiedadResponse);
        }
    }
}
