using System.Text.Json;
using MediatR;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Libros.Queries;

public class GetLibrosQueryHandler : IRequestHandler<GetLibrosQuery, PagedResult<LibroDto>>
{
    private readonly ILibroRepository _libroRepository;
    private readonly ICacheService _cacheService;

    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

    public GetLibrosQueryHandler(ILibroRepository libroRepository, ICacheService cacheService)
    {
        _libroRepository = libroRepository;
        _cacheService = cacheService;
    }

    public async Task<PagedResult<LibroDto>> Handle(GetLibrosQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = BuildCacheKey(request);

        // Intentar obtener desde Redis
        var cached = await _cacheService.GetAsync(cacheKey);
        if (cached is not null)
        {
            var deserialized = JsonSerializer.Deserialize<PagedResult<LibroDto>>(cached);
            if (deserialized is not null)
                return deserialized;
        }

        // Construir specification
        var spec = new LibroQuerySpecification(
            request.Titulo,
            request.Autor,
            request.Genero,
            request.SoloDisponibles,
            request.Pagina,
            request.TamanoPagina);

        var libros = await _libroRepository.GetAllAsync(spec);
        var totalRegistros = await _libroRepository.GetCountAsync(spec);

        var items = libros.Select(LibroDto.FromEntity).ToList();

        var result = new PagedResult<LibroDto>(
            items,
            request.Pagina,
            request.TamanoPagina,
            totalRegistros);

        // Guardar en Redis
        var serialized = JsonSerializer.Serialize(result);
        await _cacheService.SetAsync(cacheKey, serialized, CacheTtl);

        return result;
    }

    private static string BuildCacheKey(GetLibrosQuery query)
    {
        return $"libros:lista:titulo={query.Titulo}:autor={query.Autor}:genero={query.Genero}:solodisponibles={query.SoloDisponibles}:pagina={query.Pagina}:tamano={query.TamanoPagina}";
    }

    private sealed class LibroQuerySpecification : ILibroSpecification
    {
        public string? Titulo { get; }
        public string? Autor { get; }
        public string? Genero { get; }
        public bool? SoloDisponibles { get; }
        public int Pagina { get; }
        public int TamanoPagina { get; }

        public LibroQuerySpecification(
            string? titulo,
            string? autor,
            string? genero,
            bool? soloDisponibles,
            int pagina,
            int tamanoPagina)
        {
            Titulo = titulo;
            Autor = autor;
            Genero = genero;
            SoloDisponibles = soloDisponibles;
            Pagina = pagina;
            TamanoPagina = tamanoPagina;
        }
    }
}
