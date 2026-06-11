using MediatR;
using SIGEBIC.Application.Common.Exceptions;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Multas.Queries;

public class GetMultaByIdQueryHandler : IRequestHandler<GetMultaByIdQuery, MultaDto>
{
    private readonly IMultaRepository _multaRepository;

    public GetMultaByIdQueryHandler(IMultaRepository multaRepository)
    {
        _multaRepository = multaRepository;
    }

    public async Task<MultaDto> Handle(GetMultaByIdQuery request, CancellationToken cancellationToken)
    {
        var multa = await _multaRepository.GetByIdAsync(request.Id);
        if (multa is null)
            throw new NotFoundException(nameof(Multa), request.Id);

        return MultaDto.FromEntity(
            multa,
            multa.Prestamo?.Libro?.Titulo ?? "Desconocido",
            multa.Prestamo?.Usuario?.NombreCompleto() ?? "Desconocido");
    }
}