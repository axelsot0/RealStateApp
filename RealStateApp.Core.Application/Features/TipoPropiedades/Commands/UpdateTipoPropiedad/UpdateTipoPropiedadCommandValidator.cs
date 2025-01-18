using FluentValidation;

namespace RealStateApp.Core.Application.Features.TipoPropiedades.Commands.UpdateTipoPropiedad
{
    public class UpdateTipoPropiedadCommandValidator : AbstractValidator<UpdateTipoPropiedadCommand>
    {
        public UpdateTipoPropiedadCommandValidator()
        {
            RuleFor(p => p.Id)
               .NotEmpty().WithMessage("El Id es requerido")
               .GreaterThan(0).WithMessage("Id debe ser mayor a 0");

            RuleFor(p => p.Nombre)
                .NotEmpty().WithMessage("El Nombre es requerido");

            RuleFor(p => p.Descripcion)
                .NotEmpty().WithMessage("La descripcion es requerida");
        }
    }
}
