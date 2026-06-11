using MediatR;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Multas.Queries;

public class GetMultasPendientesQueryHandler : IRequestHandler<GetMultasPendientesQuery, IReadOnlyList<MultaDto>>
{
    private readonly IMultaRepository _multaRepository;

    public GetMultasPendientesQueryHandler(IMultaRepository multaRepository)
    {
        _multaRepository = multaRepository;
    }

    public async Task<IReadOnlyList<MultaDto>> Handle(GetMultasPendientesQuery request, CancellationToken cancellationToken)
    {
        var multas = await _multaRepository.GetPendientesAsync();

        return multas
            .Select(m => MultaDto.FromEntity(
                m,
                m.Prestamo?.Libro?.Titulo ?? "Desconocido",
                m.Prestamo?.Usuario?.NombreCompleto() ?? "Desconocido"))
            .ToList();
    }
}