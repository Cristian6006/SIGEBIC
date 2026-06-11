using MediatR;
using SIGEBIC.Application.Common.Exceptions;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Usuarios.Commands;

public class CambiarPasswordCommandHandler : IRequestHandler<CambiarPasswordCommand>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public CambiarPasswordCommandHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task Handle(CambiarPasswordCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(request.UsuarioId);
        if (usuario is null)
            throw new NotFoundException(nameof(Domain.Entities.Usuario), request.UsuarioId);

        // Verificar contraseña actual
        if (!BCrypt.Net.BCrypt.Verify(request.PasswordActual, usuario.PasswordHash))
            throw new UnauthorizedException("La contraseña actual no es correcta.");

        // Hashear nuevo password y aplicar método de dominio
        var nuevoHash = BCrypt.Net.BCrypt.HashPassword(request.NuevoPassword, workFactor: 12);
        usuario.CambiarPassword(nuevoHash);

        await _usuarioRepository.UpdateAsync(usuario);
    }
}
