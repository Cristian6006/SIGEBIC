using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Multas.Queries;

public record GetMultasByUsuarioQuery(
    Guid UsuarioId,
    bool? SoloPendientes,
    int Pagina = 1,
    int TamanoPagina = 10) : IRequest<PagedResult<MultaDto>>;