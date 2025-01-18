using FluentValidation;

namespace RealStateApp.Core.Application.Features.TipoVentas.Commands.CreateTipoVenta
{
    public class CreateTipoVentaCommandValidator : AbstractValidator<CreateTipoVentaCommand>
    {
        public CreateTipoVentaCommandValidator()
        {
            RuleFor(p => p.Nombre)
                .NotEmpty().WithMessage("El Nombre es requerido");

            RuleFor(p => p.Descripcion)
                .NotEmpty().WithMessage("La descripcion es requerida");
        }
    }
}
