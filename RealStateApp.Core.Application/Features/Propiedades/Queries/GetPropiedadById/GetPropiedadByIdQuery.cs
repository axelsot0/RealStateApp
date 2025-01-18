using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Propiedad;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealStateApp.Core.Application.Features.Propiedades.Queries.GetPropiedadById
{
    /// <summary>
    /// Parametros para obtener una propiedad por su Id
    /// </summary>
    public class GetPropiedadByIdQuery : IRequest<Response<PropiedadResponse>>
    {
        [SwaggerParameter(Description = "Colocar id de la propiedad que desea obtener")]
        public int Id { get; set; }
    }

    public class GetPropiedadByIdQueryHandler : IRequestHandler<GetPropiedadByIdQuery, Response<PropiedadResponse>>
    {
        private readonly IPropiedadRepository _propiedadRepo;
        private readonly IAccountServiceWebApi _accountService;
        private readonly IMapper _mapper;

        public GetPropiedadByIdQueryHandler(IPropiedadRepository propiedadRepo, IAccountServiceWebApi accountService, IMapper mapper)
        {
            _propiedadRepo = propiedadRepo;
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<Response<PropiedadResponse>> Handle(GetPropiedadByIdQuery request, CancellationToken cancellationToken)
        {
            var propiedad = await _propiedadRepo.GetByIdWithInclude(request.Id);

            if (propiedad == null) throw new ApiException($"La propiedad con el Id {request.Id} no existe", (int)HttpStatusCode.BadRequest);

            var propiedadResponse = _mapper.Map<PropiedadResponse>(propiedad);

            propiedadResponse.AgenteName = await _accountService.GetAgenteName(propiedadResponse.AgenteId);

            return new Response<PropiedadResponse>(propiedadResponse);
        }
    }
}
