using SIGEBIC.Domain.Enums;

namespace SIGEBIC.Domain.Entities;

public class Multa
{
    public Guid Id { get; private set; }
    public Guid PrestamoId { get; private set; }
    public Guid UsuarioId { get; private set; }
    public decimal MontoPorDia { get; private set; }
    public int DiasRetraso { get; private set; }
    public decimal MontoTotal { get; private set; }
    public bool Pagada { get; private set; }
    public DateTime? FechaPago { get; private set; }
    public DateTime FechaGeneracion { get; private set; }
    public string? Observaciones { get; private set; }

    // Propiedades de navegación
    public Prestamo Prestamo { get; private set; } = null!;

    // Constructor privado para EF Core / deserialización
    private Multa()
    {
    }

    // Constructor con todos los parámetros requeridos
    public Multa(
        Guid id,
        Guid prestamoId,
        Guid usuarioId,
        decimal montoPorDia,
        int diasRetraso,
        decimal montoTotal,
        string? observaciones)
    {
        if (id == default)
            throw new ArgumentException("El id es obligatorio.", nameof(id));
        if (prestamoId == default)
            throw new ArgumentException("El id del préstamo es obligatorio.", nameof(prestamoId));
        if (usuarioId == default)
            throw new ArgumentException("El id del usuario es obligatorio.", nameof(usuarioId));
        if (montoPorDia <= 0)
            throw new ArgumentException("El monto por día debe ser mayor a cero.", nameof(montoPorDia));
        if (diasRetraso <= 0)
            throw new ArgumentException("Los días de retraso deben ser mayor a cero.", nameof(diasRetraso));
        if (montoTotal <= 0)
            throw new ArgumentException("El monto total debe ser mayor a cero.", nameof(montoTotal));

        Id = id;
        PrestamoId = prestamoId;
        UsuarioId = usuarioId;
        MontoPorDia = montoPorDia;
        DiasRetraso = diasRetraso;
        MontoTotal = montoTotal;
        Pagada = false;
        FechaGeneracion = DateTime.UtcNow;
        Observaciones = observaciones;
    }

    /// <summary>
    /// Calcula el monto total de la multa según el monto por día y los días de retraso.
    /// </summary>
    public static decimal CalcularMonto(decimal montoPorDia, int diasRetraso)
    {
        return montoPorDia * diasRetraso;
    }

    /// <summary>
    /// Agrega o reemplaza las observaciones de la multa.
    /// </summary>
    public void AgregarObservaciones(string observaciones)
    {
        Observaciones = observaciones;
    }

    /// <summary>
    /// Registra el pago de la multa. Si ya estaba pagada, lanza una excepción.
    /// </summary>
    public void Pagar(DateTime fechaPago)
    {
        if (Pagada)
            throw new InvalidOperationException("Esta multa ya fue pagada.");

        Pagada = true;
        FechaPago = fechaPago;
    }
}