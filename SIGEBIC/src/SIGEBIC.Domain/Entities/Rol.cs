using SIGEBIC.Domain.Enums;

namespace SIGEBIC.Domain.Entities;

public class Rol
{
    public Guid Id { get; private set; }
    public RolNombre Nombre { get; private set; }
    public string Descripcion { get; private set; } = string.Empty;

    // Constructor privado requerido por EF Core
    private Rol()
    {
    }

    // Constructor público con parámetros requeridos
    public Rol(Guid id, RolNombre nombre, string descripcion)
    {
        Id = id;
        Nombre = nombre;
        Descripcion = descripcion;
    }
}
