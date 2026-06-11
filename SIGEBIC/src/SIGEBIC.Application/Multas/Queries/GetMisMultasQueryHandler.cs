using MediatR;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Multas.Queries;

public class GetMisMultasQueryHandler : IRequestHandler<GetMisMultasQuery, PagedResult<MultaDto>>
{
    private readonly IMultaRepository _multaRepository;

    public GetMisMultasQueryHandler(IMultaRepository multaRepository)
    {
        _multaRepository = multaRepository;
    }

    public async Task<PagedResult<MultaDto>> Handle(GetMisMultasQuery request, CancellationToken cancellationToken)
    {
        // SoloPendientes = null → el usuario ve todas sus multas, pagadas y pendientes
        var multas = await _multaRepository.GetByUsuarioAsync(
            request.UsuarioId, null, request.Pagina, request.TamanoPagina);
        var totalRegistros = await _multaRepository.GetCountByUsuarioAsync(
            request.UsuarioId, null);

        var items = multas
            .Select(m => MultaDto.FromEntity(
                m,
                m.Prestamo?.Libro?.Titulo ?? "Desconocido",
                m.Prestamo?.Usuario?.NombreCompleto() ?? "Desconocido"))
            .ToList();

        return new PagedResult<MultaDto>(items, request.Pagina, request.TamanoPagina, totalRegistros);
    }
}