using FluentValidation;

namespace RealStateApp.Core.Application.Features.TipoPropiedades.Commands.CreateTipoPropiedad
{
    public class CreateTipoPropiedadCommandValidator : AbstractValidator<CreateTipoPropiedadCommand>
    {
        public CreateTipoPropiedadCommandValidator()
        {
            RuleFor(p => p.Nombre)
                .NotEmpty().WithMessage("El Nombre es requerido");

            RuleFor(p => p.Descripcion)
                .NotEmpty().WithMessage("La descripcion es requerida");
        }
    }
}
