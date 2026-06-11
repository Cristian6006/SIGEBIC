using FluentValidation;

namespace SIGEBIC.Application.Prestamos.Commands;

public class RegistrarPrestamoCommandValidator : AbstractValidator<RegistrarPrestamoCommand>
{
    public RegistrarPrestamoCommandValidator()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty().WithMessage("El UsuarioId es requerido.")
            .NotEqual(Guid.Empty).WithMessage("El UsuarioId no puede estar vacío.");

        RuleFor(x => x.LibroId)
            .NotEmpty().WithMessage("El LibroId es requerido.")
            .NotEqual(Guid.Empty).WithMessage("El LibroId no puede estar vacío.");

        RuleFor(x => x.DiasPrestamo)
            .InclusiveBetween(1, 30)
            .WithMessage("Los días de préstamo deben estar entre 1 y 30.");
    }
}