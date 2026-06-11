using SIGEBIC.Domain.Entities;

namespace SIGEBIC.Application.DTOs;

public record RolDto(
    Guid Id,
    string Nombre,
    string Descripcion)
{
    public static RolDto FromEntity(Rol rol)
    {
        return new RolDto(
            rol.Id,
            rol.Nombre.ToString(),
            rol.Descripcion);
    }
}