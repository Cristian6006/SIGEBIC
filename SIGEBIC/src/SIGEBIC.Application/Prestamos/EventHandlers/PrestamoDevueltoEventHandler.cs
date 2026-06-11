using MediatR;
using Microsoft.Extensions.Logging;
using SIGEBIC.Application.Multas.Commands;
using SIGEBIC.Domain.Events;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Prestamos.EventHandlers;

public class PrestamoDevueltoEventHandler : INotificationHandler<PrestamoDevueltoEvent>
{
    private readonly IMediator _mediator;
    private readonly ICacheService _cacheService;
    private readonly ILogger<PrestamoDevueltoEventHandler> _logger;

    public PrestamoDevueltoEventHandler(
        IMediator mediator,
        ICacheService cacheService,
        ILogger<PrestamoDevueltoEventHandler> logger)
    {
        _mediator = mediator;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task Handle(PrestamoDevueltoEvent notification, CancellationToken cancellationToken)
    {
        // Si hubo retraso, generar la multa real
        if (notification.DiasRetraso > 0)
        {
            await _mediator.Send(new GenerarMultaCommand(
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
