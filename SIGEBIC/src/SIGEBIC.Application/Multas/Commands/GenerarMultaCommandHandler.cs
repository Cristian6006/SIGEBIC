using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SIGEBIC.Application.Common;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Multas.Commands;

public class GenerarMultaCommandHandler : IRequestHandler<GenerarMultaCommand, MultaDto?>
{
    private readonly IMultaRepository _multaRepository;
    private readonly IPrestamoRepository _prestamoRepository;
    private readonly IOptions<MultaSettings> _settings;
    private readonly ILogger<GenerarMultaCommandHandler> _logger;

    public GenerarMultaCommandHandler(
        IMultaRepository multaRepository,
        IPrestamoRepository prestamoRepository,
        IOptions<MultaSettings> settings,
        ILogger<GenerarMultaCommandHandler> logger)
    {
        _multaRepository = multaRepository;
        _prestamoRepository = prestamoRepository;
        _settings = settings;
        _logger = logger;
    }

    public async Task<MultaDto?> Handle(GenerarMultaCommand request, CancellationToken cancellationToken)
    {
        // Buscar el préstamo con Usuario y Libro incluidos
        var prestamo = await _prestamoRepository.GetByIdAsync(request.PrestamoId);
        if (prestamo is null)
        {
            _logger.LogWarning(
                "No se pudo generar multa: Préstamo {PrestamoId} no encontrado.",
                request.PrestamoId);
            return null;
        }

        // Verificar que no exista ya una multa para este préstamo (idempotencia)
        var multaExistente = await _multaRepository.GetByPrestamoAsync(request.PrestamoId);
        if (multaExistente is not null)
        {
            _logger.LogWarning(
                "Ya existe una multa para el préstamo {PrestamoId}. Retornando la existente.",
                request.PrestamoId);
            return MultaDto.FromEntity(
                multaExistente,
                prestamo.Libro?.Titulo ?? "Desconocido",
                prestamo.Usuario?.NombreCompleto() ?? "Desconocido");
        }

        // Calcular monto total
        var montoPorDia = _settings.Value.MontoPorDia;
        var montoTotal = Multa.CalcularMonto(montoPorDia, request.DiasRetraso);

        // Crear la multa
        var multa = new Multa(
            Guid.NewGuid(),
            request.PrestamoId,
            prestamo.UsuarioId,
            montoPorDia,
            request.DiasRetraso,
            montoTotal,
            null);

        await _multaRepository.AddAsync(multa);

        _logger.LogInformation(
            "Multa generada - Préstamo {PrestamoId}, Usuario {UsuarioId}, " +
            "Días de retraso: {DiasRetraso}, Monto: {MontoTotal:C2}",
            request.PrestamoId,
            prestamo.UsuarioId,
            request.DiasRetraso,
            montoTotal);

        return MultaDto.FromEntity(
            multa,
            prestamo.Libro?.Titulo ?? "Desconocido",
            prestamo.Usuario?.NombreCompleto() ?? "Desconocido");
    }
}