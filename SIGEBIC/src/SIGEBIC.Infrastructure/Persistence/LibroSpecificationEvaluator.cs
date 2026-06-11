using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Enums;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Infrastructure.Persistence;

public static class LibroSpecificationEvaluator
{
    public static IQueryable<Libro> Apply(IQueryable<Libro> query, ILibroSpecification spec)
    {
        if (!string.IsNullOrWhiteSpace(spec.Titulo))
            query = query.Where(l => l.Titulo.ToLower().Contains(spec.Titulo.ToLower()));

        if (!string.IsNullOrWhiteSpace(spec.Autor))
            query = query.Where(l => l.Autor.ToLower().Contains(spec.Autor.ToLower()));

        if (!string.IsNullOrWhiteSpace(spec.Genero))
            query = query.Where(l => l.Genero != null && l.Genero.ToLower() == spec.Genero.ToLower());

        if (spec.SoloDisponibles == true)
            query = query.Where(l => l.Estado == EstadoLibro.Disponible);

        if (spec.Pagina > 0 && spec.TamanoPagina > 0)
            query = query.Skip((spec.Pagina - 1) * spec.TamanoPagina).Take(spec.TamanoPagina);

        return query;
    }
}