using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Infrastructure.Persistence;

public class LibroSpecification : ILibroSpecification
{
    public string? Titulo { get; set; }
    public string? Autor { get; set; }
    public string? Genero { get; set; }
    public bool? SoloDisponibles { get; set; }
    public int Pagina { get; set; }
    public int TamanoPagina { get; set; }
}