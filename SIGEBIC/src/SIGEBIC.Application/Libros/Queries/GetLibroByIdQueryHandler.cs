using System.Text.Json;
using MediatR;
using SIGEBIC.Application.Common.Exceptions;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Libros.Queries;

public class GetLibroByIdQueryHandler : IRequestHandler<GetLibroByIdQuery, LibroDto>
{
    private readonly ILibroRepository _libroRepository;
    private readonly ICacheService _cacheService;

    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(10);

    public GetLibroByIdQueryHandler(ILibroRepository libroRepository, ICacheService cacheService)
    {
        _libroRepository = libroRepository;
        _cacheService = cacheService;
    }

    public async Task<LibroDto> Handle(GetLibroByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"libros:{request.Id}";

        // Intentar obtener desde Redis
        var cached = await _cacheService.GetAsync(cacheKey);
        if (cached is not null)
        {
            var deserialized = JsonSerializer.Deserialize<LibroDto>(cached);
            if (deserialized is not null)
                return deserialized;
        }

        // Buscar en base de datos
        var libro = await _libroRepository.GetByIdAsync(request.Id);
        if (libro is null)
            throw new NotFoundException("Libro", request.Id);

        var dto = LibroDto.FromEntity(libro);

        // Guardar en Redis
        var serialized = JsonSerializer.Serialize(dto);
        await _cacheService.SetAsync(cacheKey, serialized, CacheTtl);

        return dto;
    }
}