using MediatR;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Prestamos.Queries;

public class GetPrestamosVencidosQueryHandler : IRequestHandler<GetPrestamosVencidosQuery, IReadOnlyList<PrestamoDto>>
{
    private readonly IPrestamoRepository _prestamoRepository;

    public GetPrestamosVencidosQueryHandler(IPrestamoRepository prestamoRepository)
    {
        _prestamoRepository = prestamoRepository;
    }

    public async Task<IReadOnlyList<PrestamoDto>> Handle(GetPrestamosVencidosQuery request, CancellationToken cancellationToken)
    {
        var prestamos = await _prestamoRepository.GetVencidosAsync();
        return prestamos
            .Select(PrestamoDto.FromEntity)
            .ToList();
    }
}