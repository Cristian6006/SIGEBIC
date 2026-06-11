using SIGEBIC.Domain.Enums;

namespace SIGEBIC.Domain.Entities;

public class Prestamo
{
    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Guid LibroId { get; private set; }
    public DateTime FechaPrestamo { get; private set; }
    public DateTime FechaDevolucionEsperada { get; private set; }
    public DateTime? FechaDevolucionReal { get; private set; }
    public EstadoPrestamo Estado { get; private set; }
    public string? Observaciones { get; private set; }
    public int CantidadRenovaciones { get; private set; }

    // Propiedades de navegación
    public Usuario Usuario { get; private set; } = null!;
    public Libro Libro { get; private set; } = null!;

    // Constructor privado para EF Core / deserialización
    private Prestamo()
    {
    }

    // Constructor con todos los parámetros requeridos
    public Prestamo(
        Guid id,
        Guid usuarioId,
        Guid libroId,
        DateTime fechaPrestamo,
        DateTime fechaDevolucionEsperada,
        string? observaciones)
    {
        if (fechaPrestamo == default)
            throw new ArgumentException("La fecha de préstamo es obligatoria.", nameof(fechaPrestamo));
        if (fechaDevolucionEsperada == default)
            throw new ArgumentException("La fecha de devolución esperada es obligatoria.", nameof(fechaDevolucionEsperada));
        if (fechaDevolucionEsperada <= fechaPrestamo)
            throw new ArgumentException("La fecha de devolución esperada debe ser posterior a la fecha de préstamo.", nameof(fechaDevolucionEsperada));

        Id = id;
        UsuarioId = usuarioId;
        LibroId = libroId;
        FechaPrestamo = fechaPrestamo;
        FechaDevolucionEsperada = fechaDevolucionEsperada;
        Estado = EstadoPrestamo.Activo;
        Observaciones = observaciones;
        CantidadRenovaciones = 0;
    }

    /// <summary>
    /// Indica si el préstamo está vencido (activo y la fecha actual superó la esperada).
    /// </summary>
    public bool EstaVencido()
    {
        return Estado == EstadoPrestamo.Activo && DateTime.UtcNow > FechaDevolucionEsperada;
    }

    /// <summary>
    /// Calcula los días restantes para la devolución. Si está vencido retorna 0.
    /// </summary>
    public int CalcularDiasRestantes()
    {
        if (EstaVencido())
            return 0;

        return (int)(FechaDevolucionEsperada - DateTime.UtcNow).TotalDays;
    }

    /// <summary>
    /// Calcula los días de retraso. Si no se ha devuelto retorna 0.
    /// </summary>
    public int CalcularDiasRetraso()
    {
        if (FechaDevolucionReal is null)
            return 0;

        return Math.Max(0, (int)(FechaDevolucionReal.Value - FechaDevolucionEsperada).TotalDays);
    }

    /// <summary>
    /// Registra la devolución del préstamo. Determina automáticamente si fue a tiempo o vencido.
    /// </summary>
    public void Devolver(DateTime fechaReal)
    {
        if (Estado != EstadoPrestamo.Activo && Estado != EstadoPrestamo.Renovado)
            throw new InvalidOperationException("Solo se pueden devolver préstamos activos o renovados.");

        FechaDevolucionReal = fechaReal;
        int diasRetraso = CalcularDiasRetraso();

        Estado = diasRetraso > 0 ? EstadoPrestamo.Vencido : EstadoPrestamo.Devuelto;
    }

    /// <summary>
    /// Renueva el préstamo extendiendo la fecha de devolución esperada.
    /// Máximo 2 renovaciones permitidas.
    /// </summary>
    public void Renovar(int diasExtension)
    {
        if (Estado != EstadoPrestamo.Activo)
            throw new InvalidOperationException("Solo se pueden renovar préstamos activos.");

        if (CantidadRenovaciones >= 2)
            throw new InvalidOperationException("Máximo 2 renovaciones por préstamo.");

        FechaDevolucionEsperada = FechaDevolucionEsperada.AddDays(diasExtension);
        CantidadRenovaciones++;
        Estado = EstadoPrestamo.Renovado;
    }
}