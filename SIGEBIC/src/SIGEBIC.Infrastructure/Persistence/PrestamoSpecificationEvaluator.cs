using Microsoft.EntityFrameworkCore;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Enums;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Infrastructure.Persistence;

public static class PrestamoSpecificationEvaluator
{
    public static IQueryable<Prestamo> Apply(IQueryable<Prestamo> query, IPrestamoSpecification spec)
    {
        // Always include navigation properties
        query = query
            .Include(p => p.Usuario)
            .Include(p => p.Libro);

        if (spec.UsuarioId.HasValue)
            query = query.Where(p => p.UsuarioId == spec.UsuarioId.Value);

        if (spec.LibroId.HasValue)
            query = query.Where(p => p.LibroId == spec.LibroId.Value);

        if (spec.Estado.HasValue)
            query = query.Where(p => p.Estado == spec.Estado.Value);

        if (spec.Vencidos == true)
            query = query.Where(p => p.Estado == EstadoPrestamo.Activo
                                     && p.FechaDevolucionEsperada < DateTime.UtcNow);

        // Always order by most recent first
        query = query.OrderByDescending(p => p.FechaPrestamo);

        if (spec.Pagina > 0 && spec.TamanoPagina > 0)
            query = query.Skip((spec.Pagina - 1) * spec.TamanoPagina).Take(spec.TamanoPagina);

        return query;
    }
}