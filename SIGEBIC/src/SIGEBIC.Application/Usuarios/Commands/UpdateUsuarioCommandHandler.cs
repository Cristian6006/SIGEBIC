using MediatR;
using SIGEBIC.Application.Common.Exceptions;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Usuarios.Commands;

public class UpdateUsuarioCommandHandler : IRequestHandler<UpdateUsuarioCommand, UsuarioDto>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UpdateUsuarioCommandHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<UsuarioDto> Handle(UpdateUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(request.Id);
        if (usuario is null)
            throw new NotFoundException(nameof(Domain.Entities.Usuario), request.Id);

        // Verificar unicidad de documento si viene en el command
        if (!string.IsNullOrWhiteSpace(request.NumeroDocumento) &&
            request.NumeroDocumento != usuario.NumeroDocumento)
        {
            var documentoExiste = await _usuarioRepository.ExisteDocumentoAsync(request.NumeroDocumento, request.Id);
            if (documentoExiste)
                throw new ValidationException(
                    new Dictionary<string, string[]>
                    {
                        ["NumeroDocumento"] = new[] { "Ya existe un usuario con ese número de documento." }
                    });
        }

        // Actualizar solo los campos no nulos (los setters son privados, se reconstruye la entidad)
        var nombre = request.Nombre ?? usuario.Nombre;
        var apellido = request.Apellido ?? usuario.Apellido;
        var telefono = request.Telefono ?? usuario.Telefono;
        var numeroDocumento = request.NumeroDocumento ?? usuario.NumeroDocumento;

        var usuarioActualizado = new Domain.Entities.Usuario(
            usuario.Id,
            nombre,
            apellido,
            usuario.Email,
            telefono,
            numeroDocumento,
            usuario.PasswordHash,
            usuario.Activo,
            usuario.RolId);

        await _usuarioRepository.UpdateAsync(usuarioActualizado);

        return UsuarioDto.FromEntity(usuarioActualizado);
    }
}
