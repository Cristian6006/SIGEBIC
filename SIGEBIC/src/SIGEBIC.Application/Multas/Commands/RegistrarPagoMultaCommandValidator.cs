using FluentValidation;

namespace SIGEBIC.Application.Multas.Commands;

public class RegistrarPagoMultaCommandValidator : AbstractValidator<RegistrarPagoMultaCommand>
{
    public RegistrarPagoMultaCommandValidator()
    {
        RuleFor(x => x.MultaId)
            .NotEmpty().WithMessage("El MultaId es requerido.")
            .NotEqual(Guid.Empty).WithMessage("El MultaId no puede estar vacío.");
    }
}