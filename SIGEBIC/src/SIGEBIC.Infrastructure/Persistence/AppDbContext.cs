using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    // public DbSet<Usuario> Usuarios => Set<Usuario>();
    // public DbSet<Libro> Libros => Set<Libro>();
    // public DbSet<Prestamo> Prestamos => Set<Prestamo>();
    // public DbSet<HistorialPrestamo> HistorialPrestamos => Set<HistorialPrestamo>();
    // public DbSet<Multa> Multas => Set<Multa>();
    // public DbSet<Notificacion> Notificaciones => Set<Notificacion>();
    // public DbSet<Reserva> Reservas => Set<Reserva>();
    // public DbSet<Rol> Roles => Set<Rol>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}