namespace SIGEBIC.Domain.Interfaces;

public interface ILibroSpecification
{
    string? Titulo { get; }
    string? Autor { get; }
    string? Genero { get; }
    bool? SoloDisponibles { get; }
    int Pagina { get; }
    int TamanoPagina { get; }
}