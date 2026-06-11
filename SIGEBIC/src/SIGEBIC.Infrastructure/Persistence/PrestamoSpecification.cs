using SIGEBIC.Domain.Enums;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Infrastructure.Persistence;

public class PrestamoSpecification : IPrestamoSpecification
{
    public Guid? UsuarioId { get; set; }
    public Guid? LibroId { get; set; }
    public EstadoPrestamo? Estado { get; set; }
    public bool? Vencidos { get; set; }
    public int Pagina { get; set; }
    public int TamanoPagina { get; set; }
}