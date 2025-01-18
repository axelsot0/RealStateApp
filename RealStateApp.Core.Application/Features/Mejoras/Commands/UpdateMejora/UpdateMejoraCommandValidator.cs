using FluentValidation;

namespace RealStateApp.Core.Application.Features.Mejoras.Commands.UpdateMejora
{
    public class UpdateMejoraCommandValidator : AbstractValidator<UpdateMejoraCommand>
    {
        public UpdateMejoraCommandValidator()
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
