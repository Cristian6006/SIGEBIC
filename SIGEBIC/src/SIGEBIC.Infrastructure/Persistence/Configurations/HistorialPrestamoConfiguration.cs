using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBIC.Domain.Entities;

namespace SIGEBIC.Infrastructure.Persistence.Configurations;

public class HistorialPrestamoConfiguration : IEntityTypeConfiguration<HistorialPrestamo>
{
    public void Configure(EntityTypeBuilder<HistorialPrestamo> builder)
    {
        builder.ToTable("HistorialPrestamos");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.LibroId)
            .IsRequired();

        builder.HasOne(h => h.Libro)
            .WithMany()
            .HasForeignKey(h => h.LibroId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(h => h.UsuarioId)
            .IsRequired();

        builder.HasOne(h => h.Usuario)
            .WithMany()
            .HasForeignKey(h => h.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(h => h.PrestamoId)
            .IsRequired();

        builder.HasOne(h => h.Prestamo)
            .WithMany()
            .HasForeignKey(h => h.PrestamoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(h => h.FechaPrestamo)
            .IsRequired();

        builder.Property(h => h.FechaDevolucionReal)
            .IsRequired();

        builder.Property(h => h.EstadoFinal)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(h => h.DiasRetraso)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(h => h.Observaciones)
            .HasMaxLength(500)
            .IsRequired(false);

        // Índice compuesto para acelerar consultas de historial por libro
        builder.HasIndex(h => new { h.LibroId, h.FechaPrestamo })
            .HasDatabaseName("IX_HistorialPrestamos_LibroId_FechaPrestamo");

        // Índice compuesto para acelerar consultas de historial por usuario
        builder.HasIndex(h => new { h.UsuarioId, h.FechaPrestamo })
            .HasDatabaseName("IX_HistorialPrestamos_UsuarioId_FechaPrestamo");
    }
}