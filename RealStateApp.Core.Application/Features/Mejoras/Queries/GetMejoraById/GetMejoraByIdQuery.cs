using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Mejoras;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealStateApp.Core.Application.Features.Mejoras.Queries.GetMejoraById
{
    /// <summary>
    /// Parametros para obtener una mejora por Id
    /// </summary>
    public class GetMejoraByIdQuery : IRequest<Response<MejoraResponse>>
    {
        [SwaggerParameter(Description = "Colocar Id de la mejora que desea obtener")]
        public int Id { get; set; }
    }

    public class GetMejoraByIdQueryHandler : IRequestHandler<GetMejoraByIdQuery, Response<MejoraResponse>>
    {
        private readonly IMejoraRepository _mejoraRepo;
        private readonly IMapper _mapper;

        public GetMejoraByIdQueryHandler(IMejoraRepository mejoraRepo, IMapper mapper)
        {
            _mejoraRepo = mejoraRepo;
            _mapper = mapper;
        }

        public async Task<Response<MejoraResponse>> Handle(GetMejoraByIdQuery request, CancellationToken cancellationToken)
        {
            var mejora = await _mejoraRepo.GetByIdAsync(request.Id);

            if (mejora == null) throw new ApiException($"No existe mejora con el Id {request.Id}", (int)HttpStatusCode.BadRequest);

            var data = _mapper.Map<MejoraResponse>(mejora); 
            
            return new Response<MejoraResponse>(data);
        }
    }
}
