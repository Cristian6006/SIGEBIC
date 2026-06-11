using SIGEBIC.Domain.Enums;

namespace SIGEBIC.Domain.Interfaces;

public interface IPrestamoSpecification
{
    Guid? UsuarioId { get; }
    Guid? LibroId { get; }
    EstadoPrestamo? Estado { get; }
    bool? Vencidos { get; }
    int Pagina { get; }
    int TamanoPagina { get; }
}