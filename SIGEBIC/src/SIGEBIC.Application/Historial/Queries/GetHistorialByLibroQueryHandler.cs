using MediatR;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Historial.Queries;

public class GetHistorialByLibroQueryHandler : IRequestHandler<GetHistorialByLibroQuery, PagedResult<HistorialPrestamoDto>>
{
    private readonly IHistorialPrestamoRepository _historialRepository;

    public GetHistorialByLibroQueryHandler(IHistorialPrestamoRepository historialRepository)
    {
        _historialRepository = historialRepository;
    }

    public async Task<PagedResult<HistorialPrestamoDto>> Handle(GetHistorialByLibroQuery request, CancellationToken cancellationToken)
    {
        var items = await _historialRepository.GetByLibroAsync(request.LibroId, request.Pagina, request.TamanoPagina);
        var totalRegistros = await _historialRepository.GetCountByLibroAsync(request.LibroId);

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