namespace SIGEBIC.Domain.Entities;

public class Usuario
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Apellido { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Telefono { get; private set; } = string.Empty;
    public string NumeroDocumento { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime FechaRegistro { get; private set; }
    public bool Activo { get; private set; }
    public Guid RolId { get; private set; }
    public Rol Rol { get; private set; } = null!;

    // Constructor privado para EF Core / deserialización
    private Usuario()
    {
    }

    // Métodos de dominio
    public void CambiarPassword(string nuevoHash) => PasswordHash = nuevoHash;

    public void Desactivar() => Activo = false;

    public void Activar() => Activo = true;

    public void AsignarRol(Guid rolId) => RolId = rolId;

    public string NombreCompleto() => $"{Nombre} {Apellido}";

    // Constructor con todos los parámetros requeridos
    public Usuario(
        Guid id,
        string nombre,
        string apellido,
        string email,
        string telefono,
        string numeroDocumento,
        string passwordHash,
        bool activo,
        Guid rolId)
    {
        Id = id;
        Nombre = nombre;
        Apellido = apellido;
        Email = email;
        Telefono = telefono;
        NumeroDocumento = numeroDocumento;
        PasswordHash = passwordHash;
        FechaRegistro = DateTime.UtcNow;
        Activo = activo;
        RolId = rolId;
    }
}
