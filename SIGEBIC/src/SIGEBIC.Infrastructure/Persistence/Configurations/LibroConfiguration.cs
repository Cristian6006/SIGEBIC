using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Enums;

namespace SIGEBIC.Infrastructure.Persistence.Configurations;

public class LibroConfiguration : IEntityTypeConfiguration<Libro>
{
    public void Configure(EntityTypeBuilder<Libro> builder)
    {
        builder.ToTable("Libros");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.ISBN)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(l => l.ISBN)
            .IsUnique();

        builder.Property(l => l.Titulo)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(l => l.Autor)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(l => l.Editorial)
            .HasMaxLength(150)
            .IsRequired(false);

        builder.Property(l => l.Genero)
            .HasMaxLength(80)
            .IsRequired(false);

        builder.Property(l => l.Estado)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(l => l.CantidadTotal)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(l => l.CantidadDisponible)
            .HasDefaultValue(0)
            .IsRequired();

        builder.ToTable(t =>
            t.HasCheckConstraint(
                "CK_Libros_CantidadDisponible",
                "\"CantidadDisponible\" >= 0 AND \"CantidadDisponible\" <= \"CantidadTotal\""));
    }
}