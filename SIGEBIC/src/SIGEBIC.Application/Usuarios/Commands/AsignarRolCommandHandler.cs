using MediatR;
using SIGEBIC.Application.Common.Exceptions;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Usuarios.Commands;

public class AsignarRolCommandHandler : IRequestHandler<AsignarRolCommand, UsuarioDto>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IRolRepository _rolRepository;

    public AsignarRolCommandHandler(IUsuarioRepository usuarioRepository, IRolRepository rolRepository)
    {
        _usuarioRepository = usuarioRepository;
        _rolRepository = rolRepository;
    }

    public async Task<UsuarioDto> Handle(AsignarRolCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(request.UsuarioId);
        if (usuario is null)
            throw new NotFoundException(nameof(Domain.Entities.Usuario), request.UsuarioId);

        // Verificar que el rol exista
        var rol = await _rolRepository.GetByIdAsync(request.RolId);
        if (rol is null)
            throw new NotFoundException(nameof(Rol), request.RolId);

        // Aplicar método de dominio
        usuario.AsignarRol(request.RolId);

        await _usuarioRepository.UpdateAsync(usuario);

        return UsuarioDto.FromEntity(usuario);
    }
}