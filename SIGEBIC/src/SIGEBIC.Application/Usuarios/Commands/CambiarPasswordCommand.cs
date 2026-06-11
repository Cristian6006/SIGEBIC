using MediatR;

namespace SIGEBIC.Application.Usuarios.Commands;

public record CambiarPasswordCommand(
    Guid UsuarioId,
    string PasswordActual,
    string NuevoPassword) : IRequest;