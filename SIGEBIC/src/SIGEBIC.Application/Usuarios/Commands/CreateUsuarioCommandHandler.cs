using MediatR;
using SIGEBIC.Application.Common.Exceptions;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Usuarios.Commands;

public class CreateUsuarioCommandHandler : IRequestHandler<CreateUsuarioCommand, UsuarioDto>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IRolRepository _rolRepository;

    public CreateUsuarioCommandHandler(IUsuarioRepository usuarioRepository, IRolRepository rolRepository)
    {
        _usuarioRepository = usuarioRepository;
        _rolRepository = rolRepository;
    }

    public async Task<UsuarioDto> Handle(CreateUsuarioCommand request, CancellationToken cancellationToken)
    {
        // Verificar email único
        var emailExiste = await _usuarioRepository.ExisteEmailAsync(request.Email);
        if (emailExiste)
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    ["Email"] = new[] { "Ya existe un usuario con ese email." }
                });

        // Verificar documento único
        var documentoExiste = await _usuarioRepository.ExisteDocumentoAsync(request.NumeroDocumento);
        if (documentoExiste)
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    ["NumeroDocumento"] = new[] { "Ya existe un usuario con ese número de documento." }
                });

        // Verificar que el rol exista
        var rol = await _rolRepository.GetByIdAsync(request.RolId);
        if (rol is null)
            throw new NotFoundException(nameof(Rol), request.RolId);

        // Hashear el password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 12);

        var usuario = new Usuario(
            Guid.NewGuid(),
            request.Nombre,
            request.Apellido,
            request.Email,
            request.Telefono ?? string.Empty,
            request.NumeroDocumento,
            passwordHash,
            true,
            request.RolId);

        await _usuarioRepository.AddAsync(usuario);

        return UsuarioDto.FromEntity(usuario);
    }
}