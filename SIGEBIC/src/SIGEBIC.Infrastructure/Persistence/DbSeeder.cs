using Microsoft.EntityFrameworkCore;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Enums;

namespace SIGEBIC.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // Seed roles
        if (!await context.Roles.AnyAsync())
        {
            var roles = new List<Rol>
            {
                new Rol(
                    id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    nombre: RolNombre.Administrador,
                    descripcion: "Administrador del sistema con acceso completo."),
                new Rol(
                    id: Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    nombre: RolNombre.Bibliotecario,
                    descripcion: "Bibliotecario encargado de la gestión de préstamos."),
                new Rol(
                    id: Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    nombre: RolNombre.Lector,
                    descripcion: "Lector o usuario final de la biblioteca.")
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }

        // Seed admin user
        if (!await context.Usuarios.AnyAsync(u => u.Email == "admin@biblioteca.com"))
        {
            var adminRolId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var adminId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("Admin1234!");

            var admin = new Usuario(
                id: adminId,
                nombre: "Admin",
                apellido: "Biblioteca",
                email: "admin@biblioteca.com",
                telefono: "3001234567",
                numeroDocumento: "1234567890",
                passwordHash: passwordHash,
                activo: true,
                rolId: adminRolId);

            await context.Usuarios.AddAsync(admin);
            await context.SaveChangesAsync();
        }
    }
}