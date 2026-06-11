using FluentValidation;

namespace SIGEBIC.Application.Usuarios.Commands;

public class CreateUsuarioCommandValidator : AbstractValidator<CreateUsuarioCommand>
{
    public CreateUsuarioCommandValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(100).WithMessage("El nombre no debe exceder los 100 caracteres.");

        RuleFor(x => x.Apellido)
            .NotEmpty().WithMessage("El apellido es requerido.")
            .MaximumLength(100).WithMessage("El apellido no debe exceder los 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido.")
            .MaximumLength(150).WithMessage("El email no debe exceder los 150 caracteres.")
            .EmailAddress().WithMessage("El email no tiene un formato válido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida.")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .Matches(@"(?=.*[A-Z])(?=.*[a-z])(?=.*\d)")
            .WithMessage("La contraseña debe contener al menos una mayúscula, una minúscula y un número.");

        RuleFor(x => x.NumeroDocumento)
            .NotEmpty().WithMessage("El número de documento es requerido.")
            .MaximumLength(30).WithMessage("El número de documento no debe exceder los 30 caracteres.");

        RuleFor(x => x.RolId)
            .NotEmpty().WithMessage("El rol es requerido.")
            .NotEqual(Guid.Empty).WithMessage("El rol no es válido.");
    }
}