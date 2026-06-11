using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Historial.Queries;

public record GetHistorialByLibroQuery(
    Guid LibroId,
    int Pagina = 1,
    int TamanoPagina = 10) : IRequest<PagedResult<HistorialPrestamoDto>>;