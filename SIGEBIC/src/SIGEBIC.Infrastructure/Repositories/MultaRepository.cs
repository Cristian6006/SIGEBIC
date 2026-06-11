using Microsoft.EntityFrameworkCore;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Infrastructure.Repositories;

public class MultaRepository : IMultaRepository
{
    private readonly AppDbContext _context;

    public MultaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Multa?> GetByIdAsync(Guid id)
    {
        return await _context.Set<Multa>()
            .Include(m => m.Prestamo)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Multa?> GetByPrestamoAsync(Guid prestamoId)
    {
        return await _context.Set<Multa>()
            .Include(m => m.Prestamo)
            .FirstOrDefaultAsync(m => m.PrestamoId == prestamoId);
    }

    public async Task<IReadOnlyList<Multa>> GetByUsuarioAsync(Guid usuarioId, bool? soloPendientes, int pagina, int tamanoPagina)
    {
        var query = _context.Set<Multa>()
            .Include(m => m.Prestamo)
            .Where(m => m.UsuarioId == usuarioId);

        if (soloPendientes == true)
            query = query.Where(m => !m.Pagada);

        return await query
            .OrderByDescending(m => m.FechaGeneracion)
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync();
    }

    public async Task<int> GetCountByUsuarioAsync(Guid usuarioId, bool? soloPendientes)
    {
        var query = _context.Set<Multa>()
            .Where(m => m.UsuarioId == usuarioId);

        if (soloPendientes == true)
            query = query.Where(m => !m.Pagada);

        return await query.CountAsync();
    }

    public async Task<IReadOnlyList<Multa>> GetPendientesAsync()
    {
        return await _context.Set<Multa>()
            .Include(m => m.Prestamo)
            .ThenInclude(p => p.Usuario)
            .Where(m => !m.Pagada)
            .OrderByDescending(m => m.FechaGeneracion)
            .ToListAsync();
    }

    public async Task<bool> TieneMultaPendienteAsync(Guid usuarioId)
    {
        return await _context.Set<Multa>()
            .AnyAsync(m => m.UsuarioId == usuarioId && !m.Pagada);
    }

    public async Task AddAsync(Multa multa)
    {
        await _context.Set<Multa>().AddAsync(multa);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Multa multa)
    {
        _context.Set<Multa>().Update(multa);
        await _context.SaveChangesAsync();
    }
}