using SIGEBIC.Domain.Entities;

namespace SIGEBIC.Application.DTOs;

public record MultaDto(
    Guid Id,
    Guid PrestamoId,
    string TituloLibro,
    Guid UsuarioId,
    string NombreUsuario,
    decimal MontoPorDia,
    int DiasRetraso,
    decimal MontoTotal,
    bool Pagada,
    DateTime? FechaPago,
    DateTime FechaGeneracion,
    string? Observaciones)
{
    public static MultaDto FromEntity(Multa multa, string tituloLibro, string nombreUsuario)
    {
        return new MultaDto(
            multa.Id,
            multa.PrestamoId,
            tituloLibro,
            multa.UsuarioId,
            nombreUsuario,
            multa.MontoPorDia,
            multa.DiasRetraso,
            multa.MontoTotal,
            multa.Pagada,
            multa.FechaPago,
            multa.FechaGeneracion,
            multa.Observaciones);
    }
}