using MediatR;
using RealStateApp.Core.Application.Dtos.Agente;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Wrappers;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Features.Agentes.Queries.GetAllAgentes
{
    public class GetAllAgentesQuery : IRequest<Response<List<AgenteResponse>>> { }

    public class GetAllAgentesQueryHandler : IRequestHandler<GetAllAgentesQuery, Response<List<AgenteResponse>>>
    {
        private readonly IAgenteRepository _agenteRepo;
        private readonly IAccountServiceWebApi _accountService;

        public GetAllAgentesQueryHandler(IAgenteRepository agenteRepo, IAccountServiceWebApi accountService)
        {
            _agenteRepo = agenteRepo;
            _accountService = accountService;
        }

        public async Task<Response<List<AgenteResponse>>?> Handle(GetAllAgentesQuery request, CancellationToken cancellationToken)
        {
            var agentes = await _agenteRepo.GetAllWithPropertiesIncludedAsync();

            if (agentes == null || agentes.Count == 0) return null;

            var data = await GenerateAgenteResponse(agentes);

            return new Response<List<AgenteResponse>>(data);
        }

        private async Task<List<AgenteResponse>> GenerateAgenteResponse(List<Agente> agentes)
        {
            var agenteResponseList = agentes.Select(a => new AgenteResponse
            {
                Id = a.Id,
                UserId = a.UserId,
                Propiedades = a.Propiedades.Count
            })
            .ToList();

            foreach (var a in agenteResponseList)
            {
                var account = await _accountService.GetAccountByUserIdAsync(a.UserId);
                a.FirstName = account.FirstName;
                a.LastName = account.LastName;
                a.Email = account.Email;
                a.PhoneNumber = account.PhoneNumber;
            }

            return agenteResponseList;
        }
    }
}
