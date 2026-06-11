using MediatR;

namespace SIGEBIC.Domain.Events;

public record PrestamoVencidoEvent(
    Guid PrestamoId,
    Guid UsuarioId,
    Guid LibroId,
    string TituloLibro,
    string NombreUsuario,
    DateTime FechaVencimiento) : INotification;