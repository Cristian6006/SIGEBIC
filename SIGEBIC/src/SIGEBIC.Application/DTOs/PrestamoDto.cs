using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Enums;

namespace SIGEBIC.Application.DTOs;

public record PrestamoDto(
    Guid Id,
    Guid UsuarioId,
    string NombreUsuario,
    Guid LibroId,
    string TituloLibro,
    DateTime FechaPrestamo,
    DateTime FechaDevolucionEsperada,
    DateTime? FechaDevolucionReal,
    EstadoPrestamo Estado,
    int DiasRestantes,
    bool EstaVencido,
    int CantidadRenovaciones,
    string? Observaciones)
{
    public static PrestamoDto FromEntity(Prestamo prestamo)
    {
        return new PrestamoDto(
            prestamo.Id,
            prestamo.UsuarioId,
            prestamo.Usuario?.NombreCompleto() ?? "Desconocido",
            prestamo.LibroId,
            prestamo.Libro?.Titulo ?? "Desconocido",
            prestamo.FechaPrestamo,
            prestamo.FechaDevolucionEsperada,
            prestamo.FechaDevolucionReal,
            prestamo.Estado,
            prestamo.CalcularDiasRestantes(),
            prestamo.EstaVencido(),
            prestamo.CantidadRenovaciones,
            prestamo.Observaciones);
    }
}