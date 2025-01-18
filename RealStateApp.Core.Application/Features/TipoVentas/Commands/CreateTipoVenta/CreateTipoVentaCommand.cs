using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Wrappers;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Core.Application.Features.TipoVentas.Commands.CreateTipoVenta
{
    /// <summary>
    /// Parametros para la creacion de un tipo de venta
    /// </summary>
    public class CreateTipoVentaCommand : IRequest<Response<int>>
    {
        /// <example>Venta</example>
        [SwaggerParameter(Description = "Nombre del tipo de venta")]
        public string? Nombre { get; set; }

        /// <example>Venta directa a comprador final</example>
        [SwaggerParameter(Description = "Descripcion del tipo de venta")]
        public string? Descripcion { get; set; }
    }

    public class CreateTipoVentaCommandHandler : IRequestHandler<CreateTipoVentaCommand, Response<int>>
    {
        private readonly ITipoVentaRepository _tipoVentaRepo;
        private readonly IMapper _mapper;

        public CreateTipoVentaCommandHandler(ITipoVentaRepository tipoVentaRepo, IMapper mapper)
        {
            _tipoVentaRepo = tipoVentaRepo;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateTipoVentaCommand command, CancellationToken cancellationToken)
        {
            TipoVenta tipoVenta = _mapper.Map<TipoVenta>(command);

            tipoVenta = await _tipoVentaRepo.AddAsync(tipoVenta);

            return new Response<int>(tipoVenta.Id);
        }
    }
}
