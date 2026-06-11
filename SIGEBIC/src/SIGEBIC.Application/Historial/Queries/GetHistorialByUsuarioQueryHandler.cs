using MediatR;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Historial.Queries;

public class GetHistorialByUsuarioQueryHandler : IRequestHandler<GetHistorialByUsuarioQuery, PagedResult<HistorialPrestamoDto>>
{
    private readonly IHistorialPrestamoRepository _historialRepository;

    public GetHistorialByUsuarioQueryHandler(IHistorialPrestamoRepository historialRepository)
    {
        _historialRepository = historialRepository;
    }

    public async Task<PagedResult<HistorialPrestamoDto>> Handle(GetHistorialByUsuarioQuery request, CancellationToken cancellationToken)
    {
        var items = await _historialRepository.GetByUsuarioAsync(request.UsuarioId, request.Pagina, request.TamanoPagina);
        var totalRegistros = await _historialRepository.GetCountByUsuarioAsync(request.UsuarioId);

        var itemsDto = items
            .Select(HistorialPrestamoDto.FromEntity)
            .ToList();

        return new PagedResult<HistorialPrestamoDto>(
            itemsDto,
            request.Pagina,
            request.TamanoPagina,
            totalRegistros);
    }
}