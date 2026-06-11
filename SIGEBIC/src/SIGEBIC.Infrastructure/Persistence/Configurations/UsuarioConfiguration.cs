using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBIC.Domain.Entities;

namespace SIGEBIC.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Nombre)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Apellido)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Telefono)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(u => u.NumeroDocumento)
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(u => u.NumeroDocumento)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired(); // No max length specified, meaning no limit in Fluent API (defaults to max/text/nvarchar(max))

        builder.Property(u => u.FechaRegistro)
            .IsRequired();

        builder.Property(u => u.Activo)
            .IsRequired();

        // Relación muchos a uno con Rol
        builder.HasOne(u => u.Rol)
            .WithMany()
            .HasForeignKey(u => u.RolId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
