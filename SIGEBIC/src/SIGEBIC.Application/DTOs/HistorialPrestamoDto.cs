using SIGEBIC.Domain.Entities;

namespace SIGEBIC.Application.DTOs;

public record HistorialPrestamoDto(
    Guid Id,
    Guid PrestamoId,
    Guid LibroId,
    string TituloLibro,
    Guid UsuarioId,
    string NombreUsuario,
    DateTime FechaPrestamo,
    DateTime FechaDevolucionReal,
    string EstadoFinal,
    int DiasRetraso,
    string? Observaciones)
{
    public static HistorialPrestamoDto FromEntity(HistorialPrestamo h)
    {
        return new HistorialPrestamoDto(
            h.Id,
            h.PrestamoId,
            h.LibroId,
            h.Libro?.Titulo ?? "Desconocido",
            h.UsuarioId,
            h.Usuario?.NombreCompleto() ?? "Desconocido",
            h.FechaPrestamo,
            h.FechaDevolucionReal,
            h.EstadoFinal.ToString(),
            h.DiasRetraso,
            h.Observaciones);
    }
}