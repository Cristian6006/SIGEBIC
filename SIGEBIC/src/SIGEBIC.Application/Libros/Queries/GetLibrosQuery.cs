using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Libros.Queries;

public record GetLibrosQuery(
    string? Titulo,
    string? Autor,
    string? Genero,
    bool? SoloDisponibles,
    int Pagina = 1,
    int TamanoPagina = 10)
    : IRequest<PagedResult<LibroDto>>;