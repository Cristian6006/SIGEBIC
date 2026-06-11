using Microsoft.EntityFrameworkCore;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Interfaces;
using SIGEBIC.Infrastructure.Persistence;

namespace SIGEBIC.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> GetByIdAsync(Guid id)
    {
        return await _context.Set<Usuario>()
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _context.Set<Usuario>()
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await _context.Set<Usuario>()
            .Include(u => u.Rol)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Usuario>> GetAllAsync(IUsuarioSpecification spec)
    {
        var query = _context.Set<Usuario>().AsQueryable();
        query = UsuarioSpecificationEvaluator.Apply(query, spec);
        return await query.ToListAsync();
    }

    public async Task<int> GetCountAsync(IUsuarioSpecification spec)
    {
        var query = _context.Set<Usuario>().AsQueryable();
        query = UsuarioSpecificationEvaluator.Apply(query, new UsuarioSpecification
        {
            Nombre = spec.Nombre,
            Email = spec.Email,
            RolId = spec.RolId,
            Activo = spec.Activo,
            Pagina = 0,
            TamanoPagina = 0
        });
        return await query.CountAsync();
    }

    public async Task<bool> ExisteEmailAsync(string email, Guid? excludeId = null)
    {
        return await _context.Set<Usuario>()
            .AnyAsync(u => u.Email == email && (!excludeId.HasValue || u.Id != excludeId.Value));
    }

    public async Task<bool> ExisteDocumentoAsync(string documento, Guid? excludeId = null)
    {
        return await _context.Set<Usuario>()
            .AnyAsync(u => u.NumeroDocumento == documento && (!excludeId.HasValue || u.Id != excludeId.Value));
    }

    public async Task AddAsync(Usuario usuario)
    {
        await _context.Set<Usuario>().AddAsync(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Usuario usuario)
    {
        _context.Set<Usuario>().Update(usuario);
        await _context.SaveChangesAsync();
    }
}