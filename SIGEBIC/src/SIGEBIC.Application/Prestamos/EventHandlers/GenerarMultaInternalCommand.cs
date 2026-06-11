using MediatR;

namespace SIGEBIC.Application.Prestamos.EventHandlers;

public record GenerarMultaInternalCommand(
    Guid PrestamoId,
    int DiasRetraso) : IRequest;