using MediatR;
using Microsoft.Extensions.Logging;

namespace SIGEBIC.Application.Prestamos.EventHandlers;

public class GenerarMultaCommandHandler : IRequestHandler<GenerarMultaInternalCommand>
{
    private readonly ILogger<GenerarMultaCommandHandler> _logger;

    public GenerarMultaCommandHandler(ILogger<GenerarMultaCommandHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(GenerarMultaInternalCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "MULTA GENERADA - Préstamo {PrestamoId}, {DiasRetraso} días de retraso. (Implementación pendiente - Fase 6)",
            request.PrestamoId,
            request.DiasRetraso);

        return Task.CompletedTask;
    }
}