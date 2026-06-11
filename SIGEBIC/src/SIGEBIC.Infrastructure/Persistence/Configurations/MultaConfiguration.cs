using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBIC.Domain.Entities;

namespace SIGEBIC.Infrastructure.Persistence.Configurations;

public class MultaConfiguration : IEntityTypeConfiguration<Multa>
{
    public void Configure(EntityTypeBuilder<Multa> builder)
    {
        builder.ToTable("Multas");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.PrestamoId)
            .IsRequired();

        builder.HasOne(m => m.Prestamo)
            .WithMany()
            .HasForeignKey(m => m.PrestamoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(m => m.PrestamoId)
            .IsUnique()
            .HasDatabaseName("IX_Multas_PrestamoId");

        builder.Property(m => m.UsuarioId)
            .IsRequired();

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(m => m.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(m => m.MontoPorDia)
            .HasColumnType("decimal(10, 2)")
            .IsRequired();

        builder.Property(m => m.DiasRetraso)
            .IsRequired();

        builder.Property(m => m.MontoTotal)
            .HasColumnType("decimal(10, 2)")
            .IsRequired();

        builder.Property(m => m.Pagada)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(m => m.FechaPago)
            .IsRequired(false);

        builder.Property(m => m.FechaGeneracion)
            .HasDefaultValueSql("NOW()")
            .IsRequired();

        builder.Property(m => m.Observaciones)
            .HasMaxLength(500)
            .IsRequired(false);

        // Índice compuesto para acelerar consultas de multas pendientes por usuario
        builder.HasIndex(m => new { m.UsuarioId, m.Pagada })
            .HasDatabaseName("IX_Multas_UsuarioId_Pagada");
    }
}