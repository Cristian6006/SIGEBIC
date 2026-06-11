using MediatR;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Enums;

namespace SIGEBIC.Application.Prestamos.Queries;

public record GetPrestamosQuery(
    Guid? UsuarioId,
    Guid? LibroId,
    EstadoPrestamo? Estado,
    bool? Vencidos,
    int Pagina = 1,
    int TamanoPagina = 10) : IRequest<PagedResult<PrestamoDto>>;