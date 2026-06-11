using Microsoft.EntityFrameworkCore;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Interfaces;
using SIGEBIC.Infrastructure.Persistence;

namespace SIGEBIC.Infrastructure.Repositories;

public class RolRepository : IRolRepository
{
    private readonly AppDbContext _context;

    public RolRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Rol?> GetByIdAsync(Guid id)
    {
        return await _context.Set<Rol>().FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IReadOnlyList<Rol>> GetAllAsync()
    {
        return await _context.Set<Rol>().ToListAsync();
    }
}