using MediatR;

namespace SIGEBIC.Domain.Events;

public record PrestamoRegistradoEvent(
    Guid PrestamoId,
    Guid UsuarioId,
    string EmailUsuario,
    string TituloLibro,
    DateTime FechaDevolucionEsperada) : INotification;