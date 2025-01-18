using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.TipoVentas;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Wrappers;

namespace RealStateApp.Core.Application.Features.TipoVentas.Queries.GetAllTipoVentas
{
    public class GetAllTipoVentasQuery : IRequest<Response<List<TipoVentaResponse>>> { }

    public class GetAllTipoVentasQueryHandler : IRequestHandler<GetAllTipoVentasQuery, Response<List<TipoVentaResponse>>>
    {
        private readonly ITipoVentaRepository _tipoVentaRepo;
        private readonly IMapper _mapper;

        public GetAllTipoVentasQueryHandler(ITipoVentaRepository tipoVentaRepo, IMapper mapper)
        {
            _tipoVentaRepo = tipoVentaRepo;
            _mapper = mapper;
        }

        public async Task<Response<List<TipoVentaResponse>>?> Handle(GetAllTipoVentasQuery request, CancellationToken cancellationToken)
        {
            var tiposVentas = await _tipoVentaRepo.GetAllAsync();

            if (tiposVentas == null || tiposVentas.Count == 0) return null;

            var data = _mapper.Map<List<TipoVentaResponse>>(tiposVentas);

            return new Response<List<TipoVentaResponse>>(data);   
        }
    }
}
