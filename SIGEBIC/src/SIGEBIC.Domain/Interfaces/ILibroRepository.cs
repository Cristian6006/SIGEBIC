using SIGEBIC.Domain.Entities;

namespace SIGEBIC.Domain.Interfaces;

public interface ILibroRepository
{
    Task<Libro?> GetByIdAsync(Guid id);
    Task<Libro?> GetByISBNAsync(string isbn);
    Task<IReadOnlyList<Libro>> GetAllAsync(ILibroSpecification spec);
    Task<int> GetCountAsync(ILibroSpecification spec);
    Task AddAsync(Libro libro);
    Task UpdateAsync(Libro libro);
}