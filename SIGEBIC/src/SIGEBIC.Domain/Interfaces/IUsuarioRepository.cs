using SIGEBIC.Domain.Entities;

namespace SIGEBIC.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByIdAsync(Guid id);
    Task<Usuario?> GetByEmailAsync(string email);
    Task<IEnumerable<Usuario>> GetAllAsync();
    Task<IReadOnlyList<Usuario>> GetAllAsync(IUsuarioSpecification spec);
    Task<int> GetCountAsync(IUsuarioSpecification spec);
    Task<bool> ExisteEmailAsync(string email, Guid? excludeId = null);
    Task<bool> ExisteDocumentoAsync(string documento, Guid? excludeId = null);
    Task AddAsync(Usuario usuario);
    Task UpdateAsync(Usuario usuario);
}
