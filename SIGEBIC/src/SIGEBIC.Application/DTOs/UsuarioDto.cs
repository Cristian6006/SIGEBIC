using SIGEBIC.Domain.Entities;

namespace SIGEBIC.Application.DTOs;

public record UsuarioDto(
    Guid Id,
    string Nombre,
    string Apellido,
    string Email,
    string Telefono,
    string NumeroDocumento,
    DateTime FechaRegistro,
    bool Activo,
    Guid RolId,
    string NombreRol)
{
    public static UsuarioDto FromEntity(Usuario usuario)
    {
        return new UsuarioDto(
            usuario.Id,
            usuario.Nombre,
            usuario.Apellido,
            usuario.Email,
            usuario.Telefono,
            usuario.NumeroDocumento,
            usuario.FechaRegistro,
            usuario.Activo,
            usuario.RolId,
            usuario.Rol.Nombre.ToString());
    }
}
