using SIGEBIC.Domain.Entities;

namespace SIGEBIC.Domain.Interfaces;

public interface IHistorialPrestamoRepository
{
    Task AddAsync(HistorialPrestamo historial);
    Task<IReadOnlyList<HistorialPrestamo>> GetByLibroAsync(Guid libroId, int pagina, int tamanoPagina);
    Task<IReadOnlyList<HistorialPrestamo>> GetByUsuarioAsync(Guid usuarioId, int pagina, int tamanoPagina);
    Task<int> GetCountByLibroAsync(Guid libroId);
    Task<int> GetCountByUsuarioAsync(Guid usuarioId);
}