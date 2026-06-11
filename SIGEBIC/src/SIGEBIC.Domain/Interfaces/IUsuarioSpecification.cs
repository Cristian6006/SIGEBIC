namespace SIGEBIC.Domain.Interfaces;

public interface IUsuarioSpecification
{
    string? Nombre { get; }
    string? Email { get; }
    Guid? RolId { get; }
    bool? Activo { get; }
    int Pagina { get; }
    int TamanoPagina { get; }
}