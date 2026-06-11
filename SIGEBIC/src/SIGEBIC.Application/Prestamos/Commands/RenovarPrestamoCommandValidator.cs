using FluentValidation;

namespace SIGEBIC.Application.Prestamos.Commands;

public class RenovarPrestamoCommandValidator : AbstractValidator<RenovarPrestamoCommand>
{
    public RenovarPrestamoCommandValidator()
    {
        RuleFor(x => x.PrestamoId)
            .NotEmpty().WithMessage("El PrestamoId es requerido.")
            .NotEqual(Guid.Empty).WithMessage("El PrestamoId no puede estar vacío.");

        RuleFor(x => x.DiasExtension)
            .InclusiveBetween(1, 15)
            .WithMessage("Los días de extensión deben estar entre 1 y 15.");
    }
}