using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Propiedad;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealStateApp.Core.Application.Features.Propiedades.Queries.GetAllPropiedadesByAgenteId
{
    /// <summary>
    /// Parametros para filtrar propiedades por Id del agente
    /// </summary>
    public class GetAllPropiedadesByAgenteIdQuery : IRequest<Response<List<PropiedadResponse>>>
    {
        [SwaggerParameter(Description = "Colocar Id del agente")]
        public int AgenteId { get; set; }
    }

    public class GetAllPropiedadesByAgenteIdQueryHandler : IRequestHandler<GetAllPropiedadesByAgenteIdQuery, Response<List<PropiedadResponse>>>
    {
        private readonly IPropiedadRepository _propiedadRepo;
        private readonly IAccountServiceWebApi _accountService;
        private readonly IAgenteRepository _agenteRepository;
        private readonly IMapper _mapper;

        public GetAllPropiedadesByAgenteIdQueryHandler(IPropiedadRepository propiedadRepo, 
                                                       IAccountServiceWebApi accountService, 
                                                       IMapper mapper, 
                                                       IAgenteRepository agenteRepository)
        {
            _propiedadRepo = propiedadRepo;
            _accountService = accountService;
            _mapper = mapper;
            _agenteRepository = agenteRepository;
        }

        public async Task<Response<List<PropiedadResponse>>?> Handle(GetAllPropiedadesByAgenteIdQuery request, CancellationToken cancellationToken)
        {
            var agente = await _agenteRepository.GetByIdAsync(request.AgenteId);

            if(agente == null) throw new ApiException($"El agente con el Id {request.AgenteId} no existe", (int)HttpStatusCode.BadRequest);

            var propiedadesAgente = await _propiedadRepo.GetAllWithIncludeByAgenteId(request.AgenteId);

            if (propiedadesAgente == null || propiedadesAgente.Count == 0) return null;

            var propiedadResponseList = _mapper.Map<List<PropiedadResponse>>(propiedadesAgente);

            string agenteName = await _accountService.GetAgenteName(request.AgenteId);

            foreach (var p in propiedadResponseList)
            {
                p.AgenteName = agenteName;
            }

            return new Response<List<PropiedadResponse>>(propiedadResponseList);
        }
    }
}
