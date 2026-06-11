using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SIGEBIC.Infrastructure.Persistence;

/// <summary>
/// Fábrica de diseño para EF Core CLI tools (dotnet ef migrations).
/// Permite generar migraciones sin tener Redis disponible.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=biblioteca_db;Username=biblioteca_user;Password=biblioteca_pass");

        return new AppDbContext(optionsBuilder.Options);
    }
}