using MediatR;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Multas.Queries;

public class GetMultasByUsuarioQueryHandler : IRequestHandler<GetMultasByUsuarioQuery, PagedResult<MultaDto>>
{
    private readonly IMultaRepository _multaRepository;

    public GetMultasByUsuarioQueryHandler(IMultaRepository multaRepository)
    {
        _multaRepository = multaRepository;
    }

    public async Task<PagedResult<MultaDto>> Handle(GetMultasByUsuarioQuery request, CancellationToken cancellationToken)
    {
        var multas = await _multaRepository.GetByUsuarioAsync(
            request.UsuarioId, request.SoloPendientes, request.Pagina, request.TamanoPagina);
        var totalRegistros = await _multaRepository.GetCountByUsuarioAsync(
            request.UsuarioId, request.SoloPendientes);

        var items = multas
            .Select(m => MultaDto.FromEntity(
                m,
                m.Prestamo?.Libro?.Titulo ?? "Desconocido",
                m.Prestamo?.Usuario?.NombreCompleto() ?? "Desconocido"))
            .ToList();

        return new PagedResult<MultaDto>(items, request.Pagina, request.TamanoPagina, totalRegistros);
    }
}