using Microsoft.EntityFrameworkCore;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Infrastructure.Repositories;

public class HistorialPrestamoRepository : IHistorialPrestamoRepository
{
    private readonly AppDbContext _context;

    public HistorialPrestamoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(HistorialPrestamo historial)
    {
        await _context.Set<HistorialPrestamo>().AddAsync(historial);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<HistorialPrestamo>> GetByLibroAsync(Guid libroId, int pagina, int tamanoPagina)
    {
        return await _context.Set<HistorialPrestamo>()
            .Include(h => h.Usuario)
            .Include(h => h.Libro)
            .Where(h => h.LibroId == libroId)
            .OrderByDescending(h => h.FechaPrestamo)
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<HistorialPrestamo>> GetByUsuarioAsync(Guid usuarioId, int pagina, int tamanoPagina)
    {
        return await _context.Set<HistorialPrestamo>()
            .Include(h => h.Libro)
            .Include(h => h.Usuario)
            .Where(h => h.UsuarioId == usuarioId)
            .OrderByDescending(h => h.FechaPrestamo)
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync();
    }

    public async Task<int> GetCountByLibroAsync(Guid libroId)
    {
        return await _context.Set<HistorialPrestamo>()
            .CountAsync(h => h.LibroId == libroId);
    }

    public async Task<int> GetCountByUsuarioAsync(Guid usuarioId)
    {
        return await _context.Set<HistorialPrestamo>()
            .CountAsync(h => h.UsuarioId == usuarioId);
    }
}