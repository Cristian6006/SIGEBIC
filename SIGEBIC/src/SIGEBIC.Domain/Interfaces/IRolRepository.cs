using SIGEBIC.Domain.Entities;

namespace SIGEBIC.Domain.Interfaces;

public interface IRolRepository
{
    Task<Rol?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Rol>> GetAllAsync();
}
