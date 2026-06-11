using FluentValidation;

namespace SIGEBIC.Application.Libros.Commands;

public class UpdateLibroCommandValidator : AbstractValidator<UpdateLibroCommand>
{
    public UpdateLibroCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id del libro es requerido.");

        When(x => x.ISBN is not null, () =>
        {
            RuleFor(x => x.ISBN)
                .MaximumLength(20).WithMessage("El ISBN no debe exceder los 20 caracteres.");
        });

        When(x => x.Titulo is not null, () =>
        {
            RuleFor(x => x.Titulo)
                .MaximumLength(200).WithMessage("El título no debe exceder los 200 caracteres.");
        });

        When(x => x.Autor is not null, () =>
        {
            RuleFor(x => x.Autor)
                .MaximumLength(150).WithMessage("El autor no debe exceder los 150 caracteres.");
        });

        When(x => x.AnoPublicacion is not null, () =>
        {
            RuleFor(x => x.AnoPublicacion!.Value)
                .InclusiveBetween(1000, DateTime.UtcNow.Year)
                .WithMessage($"El año de publicación debe estar entre 1000 y {DateTime.UtcNow.Year}.");
        });

        When(x => x.CantidadTotal is not null, () =>
        {
            RuleFor(x => x.CantidadTotal!.Value)
                .GreaterThan(0).WithMessage("La cantidad total debe ser mayor a 0.");
        });
    }
}