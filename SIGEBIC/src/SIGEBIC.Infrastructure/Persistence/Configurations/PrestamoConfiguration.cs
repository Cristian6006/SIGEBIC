using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Enums;

namespace SIGEBIC.Infrastructure.Persistence.Configurations;

public class PrestamoConfiguration : IEntityTypeConfiguration<Prestamo>
{
    public void Configure(EntityTypeBuilder<Prestamo> builder)
    {
        builder.ToTable("Prestamos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.UsuarioId)
            .IsRequired();

        builder.HasOne(p => p.Usuario)
            .WithMany()
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.LibroId)
            .IsRequired();

        builder.HasOne(p => p.Libro)
            .WithMany()
            .HasForeignKey(p => p.LibroId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.FechaPrestamo)
            .HasDefaultValueSql("NOW()")
            .IsRequired();

        builder.Property(p => p.FechaDevolucionEsperada)
            .IsRequired();

        builder.Property(p => p.FechaDevolucionReal)
            .IsRequired(false);

        builder.Property(p => p.Estado)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.Observaciones)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(p => p.CantidadRenovaciones)
            .HasDefaultValue(0)
            .IsRequired();
    }
}