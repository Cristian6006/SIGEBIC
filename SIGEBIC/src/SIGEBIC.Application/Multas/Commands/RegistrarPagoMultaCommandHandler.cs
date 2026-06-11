using MediatR;
using SIGEBIC.Application.Common.Exceptions;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Multas.Commands;

public class RegistrarPagoMultaCommandHandler : IRequestHandler<RegistrarPagoMultaCommand, MultaDto>
{
    private readonly IMultaRepository _multaRepository;

    public RegistrarPagoMultaCommandHandler(IMultaRepository multaRepository)
    {
        _multaRepository = multaRepository;
    }

    public async Task<MultaDto> Handle(RegistrarPagoMultaCommand request, CancellationToken cancellationToken)
    {
        var multa = await _multaRepository.GetByIdAsync(request.MultaId);
        if (multa is null)
            throw new NotFoundException(nameof(Multa), request.MultaId);

        // El método de dominio Pagar valida que no esté ya pagada
        multa.Pagar(DateTime.UtcNow);

        if (request.Observaciones is not null)
        {
            multa.AgregarObservaciones(request.Observaciones);
        }

        await _multaRepository.UpdateAsync(multa);

        return MultaDto.FromEntity(
            multa,
            multa.Prestamo?.Libro?.Titulo ?? "Desconocido",
            multa.Prestamo?.Usuario?.NombreCompleto() ?? "Desconocido");
    }
}