using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Enums;

namespace SIGEBIC.Application.DTOs;

public record LibroDto(
    Guid Id,
    string ISBN,
    string Titulo,
    string Autor,
    string? Editorial,
    int AnoPublicacion,
    string? Genero,
    int CantidadTotal,
    int CantidadDisponible,
    bool EstaDisponible,
    EstadoLibro Estado)
{
    public static LibroDto FromEntity(Libro libro)
    {
        return new LibroDto(
            libro.Id,
            libro.ISBN,
            libro.Titulo,
            libro.Autor,
            libro.Editorial,
            libro.AnoPublicacion,
            libro.Genero,
            libro.CantidadTotal,
            libro.CantidadDisponible,
            libro.EstaDisponible(),
            libro.Estado);
    }
}