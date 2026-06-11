using MediatR;
using Microsoft.Extensions.Logging;
using SIGEBIC.Domain.Events;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Prestamos.EventHandlers;

public class PrestamoDevueltoEventHandler : INotificationHandler<PrestamoDevueltoEvent>
{
    private readonly IPublisher _publisher;
    private readonly ICacheService _cacheService;
    private readonly ILogger<PrestamoDevueltoEventHandler> _logger;

    public PrestamoDevueltoEventHandler(
        IPublisher publisher,
        ICacheService cacheService,
        ILogger<PrestamoDevueltoEventHandler> logger)
    {
        _publisher = publisher;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task Handle(PrestamoDevueltoEvent notification, CancellationToken cancellationToken)
    {
        // Si hubo retraso, disparar el comando interno para generar multa
        if (notification.DiasRetraso > 0)
        {
            await _publisher.Publish(new GenerarMultaInternalCommand(
                notification.PrestamoId,
                notification.DiasRetraso), cancellationToken);
        }

        // Invalidar caché del libro
        await _cacheService.DeleteAsync($"libros:{notification.LibroId}");

        _logger.LogInformation(
            "Préstamo {PrestamoId} devuelto. Libro {LibroId}, Días de retraso: {DiasRetraso}",
            notification.PrestamoId,
            notification.LibroId,
            notification.DiasRetraso);
    }
}