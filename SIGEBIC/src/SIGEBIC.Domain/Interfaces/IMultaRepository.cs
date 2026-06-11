using SIGEBIC.Domain.Entities;

namespace SIGEBIC.Domain.Interfaces;

public interface IMultaRepository
{
    Task<Multa?> GetByIdAsync(Guid id);
    Task<Multa?> GetByPrestamoAsync(Guid prestamoId);
    Task<IReadOnlyList<Multa>> GetByUsuarioAsync(Guid usuarioId, bool? soloPendientes, int pagina, int tamanoPagina);
    Task<int> GetCountByUsuarioAsync(Guid usuarioId, bool? soloPendientes);
    Task<IReadOnlyList<Multa>> GetPendientesAsync();
    Task<bool> TieneMultaPendienteAsync(Guid usuarioId);
    Task AddAsync(Multa multa);
    Task UpdateAsync(Multa multa);
}