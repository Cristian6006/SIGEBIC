using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Libros.Commands;

public record UpdateLibroCommand(
    Guid Id,
    string? ISBN,
    string? Titulo,
    string? Autor,
    string? Editorial,
    int? AnoPublicacion,
    string? Genero,
    int? CantidadTotal)
    : IRequest<LibroDto>;