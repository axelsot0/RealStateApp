using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.TipoPropiedades;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Wrappers;

namespace RealStateApp.Core.Application.Features.TipoPropiedades.Queries.GetAllTipoPropiedades
{
    public class GetAllTipoPropiedadesQuery : IRequest<Response<List<TipoPropiedadResponse>>> { }

    public class GetAllTipoPropiedadesQueryHandler : IRequestHandler<GetAllTipoPropiedadesQuery, Response<List<TipoPropiedadResponse>>>
    {
        private readonly ITipoPropiedadRepository _tipoPropiedadRepo;
        private readonly IMapper _mapper;

        public GetAllTipoPropiedadesQueryHandler(ITipoPropiedadRepository tipoPropiedadRepo, IMapper mapper)
        {
            _tipoPropiedadRepo = tipoPropiedadRepo;
            _mapper = mapper;
        }

        public async Task<Response<List<TipoPropiedadResponse>>?> Handle(GetAllTipoPropiedadesQuery request, CancellationToken cancellationToken)
        {
            var tiposPropiedades = await _tipoPropiedadRepo.GetAllAsync();

            if (tiposPropiedades == null || tiposPropiedades.Count == 0) return null;

            var data = _mapper.Map<List<TipoPropiedadResponse>>(tiposPropiedades);

            return new Response<List<TipoPropiedadResponse>>(data);
        }
    }
}
