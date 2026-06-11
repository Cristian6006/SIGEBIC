using MediatR;

namespace SIGEBIC.Domain.Events;

public record PrestamoDevueltoEvent(
    Guid PrestamoId,
    Guid LibroId,
    Guid UsuarioId,
    int DiasRetraso,
    DateTime FechaDevolucionReal,
    string? Observaciones) : INotification;