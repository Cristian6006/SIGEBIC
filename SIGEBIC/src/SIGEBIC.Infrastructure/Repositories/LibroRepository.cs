using Microsoft.EntityFrameworkCore;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Interfaces;
using SIGEBIC.Infrastructure.Persistence;

namespace SIGEBIC.Infrastructure.Repositories;

public class LibroRepository : ILibroRepository
{
    private readonly AppDbContext _context;

    public LibroRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Libro?> GetByIdAsync(Guid id)
    {
        return await _context.Set<Libro>()
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<Libro?> GetByISBNAsync(string isbn)
    {
        return await _context.Set<Libro>()
            .FirstOrDefaultAsync(l => l.ISBN == isbn);
    }

    public async Task<IReadOnlyList<Libro>> GetAllAsync(ILibroSpecification spec)
    {
        var query = _context.Set<Libro>().AsQueryable();
        query = LibroSpecificationEvaluator.Apply(query, spec);
        return await query.ToListAsync();
    }

    public async Task<int> GetCountAsync(ILibroSpecification spec)
    {
        var query = _context.Set<Libro>().AsQueryable();
        // Apply only the Where filters without pagination
        query = LibroSpecificationEvaluator.Apply(query, new LibroSpecification
        {
            Titulo = spec.Titulo,
            Autor = spec.Autor,
            Genero = spec.Genero,
            SoloDisponibles = spec.SoloDisponibles,
            Pagina = 0,
            TamanoPagina = 0
        });
        return await query.CountAsync();
    }

    public async Task AddAsync(Libro libro)
    {
        await _context.Set<Libro>().AddAsync(libro);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Libro libro)
    {
        _context.Set<Libro>().Update(libro);
        await _context.SaveChangesAsync();
    }
}