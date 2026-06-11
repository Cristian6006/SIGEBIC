using MediatR;
using SIGEBIC.Application.Common.Exceptions;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Usuarios.Commands;

public class ToggleActivoCommandHandler : IRequestHandler<ToggleActivoCommand, UsuarioDto>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public ToggleActivoCommandHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<UsuarioDto> Handle(ToggleActivoCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(request.UsuarioId);
        if (usuario is null)
            throw new NotFoundException(nameof(Domain.Entities.Usuario), request.UsuarioId);

        // Aplicar método de dominio
        if (request.Activar)
            usuario.Activar();
        else
            usuario.Desactivar();

        await _usuarioRepository.UpdateAsync(usuario);

        return UsuarioDto.FromEntity(usuario);
    }
}