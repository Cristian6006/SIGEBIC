using SIGEBIC.Domain.Enums;

namespace SIGEBIC.Domain.Entities;

public class Libro
{
    public Guid Id { get; private set; }
    public string ISBN { get; private set; } = string.Empty;
    public string Titulo { get; private set; } = string.Empty;
    public string Autor { get; private set; } = string.Empty;
    public string Editorial { get; private set; } = string.Empty;
    public int AnoPublicacion { get; private set; }
    public string Genero { get; private set; } = string.Empty;
    public int CantidadTotal { get; private set; }
    public int CantidadDisponible { get; private set; }
    public EstadoLibro Estado { get; private set; }

    // Constructor privado para EF Core / deserialización
    private Libro()
    {
    }

    // Constructor con todos los parámetros requeridos
    public Libro(
        Guid id,
        string isbn,
        string titulo,
        string autor,
        string editorial,
        int anoPublicacion,
        string genero,
        int cantidadTotal)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("El ISBN es obligatorio.", nameof(isbn));
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("El título es obligatorio.", nameof(titulo));
        if (string.IsNullOrWhiteSpace(autor))
            throw new ArgumentException("El autor es obligatorio.", nameof(autor));
        if (cantidadTotal <= 0)
            throw new ArgumentException("La cantidad total debe ser mayor a cero.", nameof(cantidadTotal));

        Id = id;
        ISBN = isbn;
        Titulo = titulo;
        Autor = autor;
        Editorial = editorial;
        AnoPublicacion = anoPublicacion;
        Genero = genero;
        CantidadTotal = cantidadTotal;
        CantidadDisponible = cantidadTotal;
        Estado = EstadoLibro.Disponible;
    }

    /// <summary>
    /// Indica si el libro está disponible para préstamo.
    /// </summary>
    public bool EstaDisponible()
    {
        return CantidadDisponible > 0 && Estado == EstadoLibro.Disponible;
    }

    /// <summary>
    /// Actualiza el estado del libro según la cantidad disponible.
    /// </summary>
    public void ActualizarDisponibilidad()
    {
        if (CantidadDisponible == 0)
        {
            Estado = EstadoLibro.Prestado;
        }
        else if (CantidadDisponible > 0 && Estado == EstadoLibro.Prestado)
        {
            Estado = EstadoLibro.Disponible;
        }
    }

    /// <summary>
    /// Descuenta un ejemplar de la cantidad disponible. Lanza excepción si no hay disponibilidad.
    /// </summary>
    public void DescontarEjemplar()
    {
        if (!EstaDisponible())
            throw new InvalidOperationException("No hay ejemplares disponibles del libro.");

        CantidadDisponible--;
        ActualizarDisponibilidad();
    }

    /// <summary>
    /// Incrementa en 1 la cantidad disponible al devolver un ejemplar.
    /// </summary>
    public void DevolverEjemplar()
    {
        CantidadDisponible++;
        ActualizarDisponibilidad();
    }

    /// <summary>
    /// Da de baja el libro, cambiando su estado a DeBaja.
    /// </summary>
    public void DarDeBaja()
    {
        Estado = EstadoLibro.DeBaja;
    }
}