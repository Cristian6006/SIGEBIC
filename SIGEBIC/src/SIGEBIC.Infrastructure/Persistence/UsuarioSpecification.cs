using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Infrastructure.Persistence;

public class UsuarioSpecification : IUsuarioSpecification
{
    public string? Nombre { get; set; }
    public string? Email { get; set; }
    public Guid? RolId { get; set; }
    public bool? Activo { get; set; }
    public int Pagina { get; set; }
    public int TamanoPagina { get; set; }
}