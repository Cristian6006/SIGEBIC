using SIGEBIC.Domain.Entities;

namespace SIGEBIC.Domain.Interfaces;

public interface IPrestamoRepository
{
    Task<Prestamo?> GetByIdAsync(Guid id);
    Task<Prestamo?> GetActivoByUsuarioYLibroAsync(Guid usuarioId, Guid libroId);
    Task<IReadOnlyList<Prestamo>> GetAllAsync(IPrestamoSpecification spec);
    Task<int> GetCountAsync(IPrestamoSpecification spec);
    Task<IReadOnlyList<Prestamo>> GetVencidosAsync();
    Task<IReadOnlyList<Prestamo>> GetProximosAVencerAsync(int diasAnticipacion);
    Task AddAsync(Prestamo prestamo);
    Task UpdateAsync(Prestamo prestamo);
}