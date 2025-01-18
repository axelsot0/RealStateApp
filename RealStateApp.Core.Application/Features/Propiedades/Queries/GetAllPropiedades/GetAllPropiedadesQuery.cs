using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Propiedad;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Wrappers;

namespace RealStateApp.Core.Application.Features.Propiedades.Queries.GetAllPropiedades
{
    public class GetAllPropiedadesQuery : IRequest<Response<List<PropiedadResponse>>> { }

    public class GetAllPropiedadesQueryHandler : IRequestHandler<GetAllPropiedadesQuery, Response<List<PropiedadResponse>>>
    {
        private readonly IPropiedadRepository _propiedadRepo;
        private readonly IAccountServiceWebApi _accountService;
        private readonly IMapper _mapper;

        public GetAllPropiedadesQueryHandler(IPropiedadRepository propiedadRepo, IAccountServiceWebApi accountService, IMapper mapper)
        {
            _propiedadRepo = propiedadRepo;
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<Response<List<PropiedadResponse>>?> Handle(GetAllPropiedadesQuery request, CancellationToken cancellationToken)
        {
            var propiedades = await _propiedadRepo.GetAllWithInclude();

            if (propiedades == null || propiedades.Count == 0) return null;

            var propiedadesResponse = _mapper.Map<List<PropiedadResponse>>(propiedades);

            foreach (var p in propiedadesResponse)
            {
                p.AgenteName = await _accountService.GetAgenteName(p.AgenteId);
            }

            return new Response<List<PropiedadResponse>>(propiedadesResponse);
        }
    }
}
