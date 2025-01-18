using FluentValidation;

namespace RealStateApp.Core.Application.Features.Mejoras.Commands.CreateMejora
{
    public class CreateMejoraCommandValidator : AbstractValidator<CreateMejoraCommand>
    {
        public CreateMejoraCommandValidator()
        {
            RuleFor(p => p.Nombre)
                .NotEmpty().WithMessage("El Nombre es requerido");

            RuleFor(p => p.Descripcion)
                .NotEmpty().WithMessage("La descripcion es requerida");
        }
    }
}
