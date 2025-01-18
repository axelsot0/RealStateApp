using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.TipoVentas;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealStateApp.Core.Application.Features.TipoVentas.Queries.GetTipoVentaById
{
    /// <summary>
    /// Parametros para obtener un tipo de venta por su Id
    /// </summary>
    public class GetTipoVentaByIdQuery : IRequest<Response<TipoVentaResponse>>
    {
        [SwaggerParameter(Description = "Colocar Id del tipo de venta que desea obtener")]
        public int Id { get; set; }
    }

    public class GetTipoVentaByIdQueryHandler : IRequestHandler<GetTipoVentaByIdQuery, Response<TipoVentaResponse>>
    {
        private readonly ITipoVentaRepository _tipoVentaRepo;
        private readonly IMapper _mapper;

        public GetTipoVentaByIdQueryHandler(ITipoVentaRepository tipoVentaRepo, IMapper mapper)
        {
            _tipoVentaRepo = tipoVentaRepo;
            _mapper = mapper;
        }

        public async Task<Response<TipoVentaResponse>> Handle(GetTipoVentaByIdQuery request, CancellationToken cancellationToken)
        {
            var tipoVenta = await _tipoVentaRepo.GetByIdAsync(request.Id);

            if (tipoVenta == null) throw new ApiException($"No existe tipo de venta con el Id {request.Id}", (int)HttpStatusCode.BadRequest);

            var data = _mapper.Map<TipoVentaResponse>(tipoVenta);

            return new Response<TipoVentaResponse>(data);
        }
    }
}
