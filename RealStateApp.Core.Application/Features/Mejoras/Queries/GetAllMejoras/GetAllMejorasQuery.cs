using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Mejoras;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Wrappers;

namespace RealStateApp.Core.Application.Features.Mejoras.Queries.GetAllMejoras
{
    public class GetAllMejorasQuery : IRequest<Response<List<MejoraResponse>>> { }

    public class GetAllMejorasQueryHandler : IRequestHandler<GetAllMejorasQuery, Response<List<MejoraResponse>>>
    {
        private readonly IMejoraRepository _mejoraRepo;
        private readonly IMapper _mapper;

        public GetAllMejorasQueryHandler(IMejoraRepository mejoraRepo, IMapper mapper)
        {
            _mejoraRepo = mejoraRepo;
            _mapper = mapper;
        }

        public async Task<Response<List<MejoraResponse>>?> Handle(GetAllMejorasQuery request, CancellationToken cancellationToken)
        {
            var mejoras = await _mejoraRepo.GetAllAsync();

            if (mejoras == null || mejoras.Count == 0) return null;

            var data =  _mapper.Map<List<MejoraResponse>>(mejoras);

            return new Response<List<MejoraResponse>>(data);
        }
    }
}
