using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Agente;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Wrappers;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealStateApp.Core.Application.Features.Agentes.Queries.GetAgenteById
{
    
    public class GetAgenteByIdQuery : IRequest<Response<AgenteResponse>>
    {
        [SwaggerParameter(Description = "Colocar Id del agente que desea obtener")]
        public int Id { get; set; }
    }

    public class GetAgenteByIdQueryHandler : IRequestHandler<GetAgenteByIdQuery, Response<AgenteResponse>>
    {
        private readonly IAgenteRepository _agenteRepo;
        private readonly IAccountServiceWebApi _accountService;
        private readonly IMapper _mapper;

        public GetAgenteByIdQueryHandler(IAgenteRepository agenteRepo, IAccountServiceWebApi accountService, IMapper mapper)
        {
            _agenteRepo = agenteRepo;
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<Response<AgenteResponse>> Handle(GetAgenteByIdQuery request, CancellationToken cancellationToken)
        {
            var agente = await _agenteRepo.GetByIdWithPropertiesIncludedAsync(request.Id);

            if (agente == null) throw new ApiException($"El agente con el Id {request.Id} no existe",(int)HttpStatusCode.BadRequest);

            var data = await GenerateAgenteResponse(agente);

            return new Response<AgenteResponse>(data);

        }

        private async Task<AgenteResponse> GenerateAgenteResponse(Agente agente)
        {
            var account = await _accountService.GetAccountByUserIdAsync(agente.UserId);

            return new AgenteResponse
            {
                Id = agente.Id,
                UserId = agente.UserId,
                Propiedades = agente.Propiedades.Count,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
            };
        }
    }
}
