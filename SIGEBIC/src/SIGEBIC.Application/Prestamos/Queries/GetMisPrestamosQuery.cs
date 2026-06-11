using MediatR;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Enums;

namespace SIGEBIC.Application.Prestamos.Queries;

public record GetMisPrestamosQuery(
    Guid UsuarioId,
    EstadoPrestamo? Estado,
    int Pagina = 1,
    int TamanoPagina = 10) : IRequest<PagedResult<PrestamoDto>>;