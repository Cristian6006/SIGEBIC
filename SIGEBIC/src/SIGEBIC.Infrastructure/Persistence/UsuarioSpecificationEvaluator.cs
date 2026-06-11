using Microsoft.EntityFrameworkCore;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Infrastructure.Persistence;

public static class UsuarioSpecificationEvaluator
{
    public static IQueryable<Usuario> Apply(IQueryable<Usuario> query, IUsuarioSpecification spec)
    {
        query = query.Include(u => u.Rol);

        if (!string.IsNullOrWhiteSpace(spec.Nombre))
        {
            var nombreLower = spec.Nombre.ToLower();
            query = query.Where(u =>
                u.Nombre.ToLower().Contains(nombreLower) ||
                u.Apellido.ToLower().Contains(nombreLower));
        }

        if (!string.IsNullOrWhiteSpace(spec.Email))
        {
            var emailLower = spec.Email.ToLower();
            query = query.Where(u => u.Email.ToLower().Contains(emailLower));
        }

        if (spec.RolId.HasValue)
            query = query.Where(u => u.RolId == spec.RolId.Value);

        if (spec.Activo.HasValue)
            query = query.Where(u => u.Activo == spec.Activo.Value);

        query = query.OrderBy(u => u.Apellido);

        if (spec.Pagina > 0 && spec.TamanoPagina > 0)
            query = query.Skip((spec.Pagina - 1) * spec.TamanoPagina).Take(spec.TamanoPagina);

        return query;
    }
}