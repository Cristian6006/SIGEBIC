using SIGEBIC.Domain.Enums;

namespace SIGEBIC.Domain.Entities;

public class HistorialPrestamo
{
    public Guid Id { get; private set; }
    public Guid LibroId { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Guid PrestamoId { get; private set; }
    public DateTime FechaPrestamo { get; private set; }
    public DateTime FechaDevolucionReal { get; private set; }
    public EstadoPrestamo EstadoFinal { get; private set; }
    public int DiasRetraso { get; private set; }
    public string? Observaciones { get; private set; }

    // Propiedades de navegación
    public Libro Libro { get; private set; } = null!;
    public Usuario Usuario { get; private set; } = null!;
    public Prestamo Prestamo { get; private set; } = null!;

    // Constructor privado para EF Core / deserialización
    private HistorialPrestamo()
    {
    }

    // Constructor con todos los parámetros requeridos
    public HistorialPrestamo(
        Guid id,
        Guid libroId,
        Guid usuarioId,
        Guid prestamoId,
        DateTime fechaPrestamo,
        DateTime fechaDevolucionReal,
        EstadoPrestamo estadoFinal,
        int diasRetraso,
        string? observaciones)
    {
        if (fechaPrestamo == default)
            throw new ArgumentException("La fecha de préstamo es obligatoria.", nameof(fechaPrestamo));
        if (fechaDevolucionReal == default)
            throw new ArgumentException("La fecha de devolución real es obligatoria.", nameof(fechaDevolucionReal));

        Id = id;
        LibroId = libroId;
        UsuarioId = usuarioId;
        PrestamoId = prestamoId;
        FechaPrestamo = fechaPrestamo;
        FechaDevolucionReal = fechaDevolucionReal;
        EstadoFinal = estadoFinal;
        DiasRetraso = diasRetraso;
        Observaciones = observaciones;
    }

    /// <summary>
    /// Crea un HistorialPrestamo a partir de un Prestamo devuelto.
    /// </summary>
    public static HistorialPrestamo CrearDesdePrestamo(Prestamo prestamo)
    {
        if (prestamo.FechaDevolucionReal is null)
            throw new InvalidOperationException("No se puede crear historial de un préstamo no devuelto.");

        return new HistorialPrestamo(
            Guid.NewGuid(),
            prestamo.LibroId,
            prestamo.UsuarioId,
            prestamo.Id,
            prestamo.FechaPrestamo,
            prestamo.FechaDevolucionReal.Value,
            prestamo.Estado,
            prestamo.CalcularDiasRetraso(),
            prestamo.Observaciones);
    }
}