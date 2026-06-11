using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Multas.Queries;

public record GetMisMultasQuery(
    Guid UsuarioId,
    int Pagina = 1,
    int TamanoPagina = 10) : IRequest<PagedResult<MultaDto>>;