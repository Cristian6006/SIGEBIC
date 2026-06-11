using Microsoft.EntityFrameworkCore;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Enums;
using SIGEBIC.Domain.Interfaces;
using SIGEBIC.Infrastructure.Persistence;

namespace SIGEBIC.Infrastructure.Repositories;

public class PrestamoRepository : IPrestamoRepository
{
    private readonly AppDbContext _context;

    public PrestamoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Prestamo?> GetByIdAsync(Guid id)
    {
        return await _context.Set<Prestamo>()
            .Include(p => p.Usuario)
            .Include(p => p.Libro)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Prestamo?> GetActivoByUsuarioYLibroAsync(Guid usuarioId, Guid libroId)
    {
        return await _context.Set<Prestamo>()
            .Include(p => p.Usuario)
            .Include(p => p.Libro)
            .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId
                                      && p.LibroId == libroId
                                      && p.Estado == EstadoPrestamo.Activo);
    }

    public async Task<IReadOnlyList<Prestamo>> GetAllAsync(IPrestamoSpecification spec)
    {
        var query = _context.Set<Prestamo>().AsQueryable();
        query = PrestamoSpecificationEvaluator.Apply(query, spec);
        return await query.ToListAsync();
    }

    public async Task<int> GetCountAsync(IPrestamoSpecification spec)
    {
        var query = _context.Set<Prestamo>().AsQueryable();
        query = PrestamoSpecificationEvaluator.Apply(query, new PrestamoSpecification
        {
            UsuarioId = spec.UsuarioId,
            LibroId = spec.LibroId,
            Estado = spec.Estado,
            Vencidos = spec.Vencidos,
            Pagina = 0,
            TamanoPagina = 0
        });
        return await query.CountAsync();
    }

    public async Task<IReadOnlyList<Prestamo>> GetVencidosAsync()
    {
        return await _context.Set<Prestamo>()
            .Include(p => p.Usuario)
            .Include(p => p.Libro)
            .Where(p => p.Estado == EstadoPrestamo.Activo
                        && p.FechaDevolucionEsperada < DateTime.UtcNow)
            .OrderByDescending(p => p.FechaDevolucionEsperada)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Prestamo>> GetProximosAVencerAsync(int diasAnticipacion)
    {
        var limiteSuperior = DateTime.UtcNow.AddDays(diasAnticipacion);
        return await _context.Set<Prestamo>()
            .Include(p => p.Usuario)
            .Include(p => p.Libro)
            .Where(p => p.Estado == EstadoPrestamo.Activo
                        && p.FechaDevolucionEsperada >= DateTime.UtcNow
                        && p.FechaDevolucionEsperada <= limiteSuperior)
            .OrderBy(p => p.FechaDevolucionEsperada)
            .ToListAsync();
    }

    public async Task AddAsync(Prestamo prestamo)
    {
        await _context.Set<Prestamo>().AddAsync(prestamo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Prestamo prestamo)
    {
        _context.Set<Prestamo>().Update(prestamo);
        await _context.SaveChangesAsync();
    }
}