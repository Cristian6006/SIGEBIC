using FluentValidation;

namespace SIGEBIC.Application.Libros.Commands;

public class CreateLibroCommandValidator : AbstractValidator<CreateLibroCommand>
{
    public CreateLibroCommandValidator()
    {
        RuleFor(x => x.ISBN)
            .NotEmpty().WithMessage("El ISBN es requerido.")
            .MaximumLength(20).WithMessage("El ISBN no debe exceder los 20 caracteres.");

        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("El título es requerido.")
            .MaximumLength(200).WithMessage("El título no debe exceder los 200 caracteres.");

        RuleFor(x => x.Autor)
            .NotEmpty().WithMessage("El autor es requerido.")
            .MaximumLength(150).WithMessage("El autor no debe exceder los 150 caracteres.");

        RuleFor(x => x.AnoPublicacion)
            .InclusiveBetween(1000, DateTime.UtcNow.Year)
            .WithMessage($"El año de publicación debe estar entre 1000 y {DateTime.UtcNow.Year}.");

        RuleFor(x => x.CantidadTotal)
            .GreaterThan(0).WithMessage("La cantidad total debe ser mayor a 0.");
    }
}