using System.Reflection;
using MediatR;
using SIGEBIC.Application.Common.Exceptions;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Libros.Commands;

public class UpdateLibroCommandHandler : IRequestHandler<UpdateLibroCommand, LibroDto>
{
    private readonly ILibroRepository _libroRepository;
    private readonly ICacheService _cacheService;

    public UpdateLibroCommandHandler(ILibroRepository libroRepository, ICacheService cacheService)
    {
        _libroRepository = libroRepository;
        _cacheService = cacheService;
    }

    public async Task<LibroDto> Handle(UpdateLibroCommand request, CancellationToken cancellationToken)
    {
        var libro = await _libroRepository.GetByIdAsync(request.Id);
        if (libro is null)
            throw new NotFoundException("Libro", request.Id);

        // Actualizar solo los campos que vienen en el command
        SetIfNotNull(libro, nameof(libro.ISBN), request.ISBN);
        SetIfNotNull(libro, nameof(libro.Titulo), request.Titulo);
        SetIfNotNull(libro, nameof(libro.Autor), request.Autor);
        SetIfNotNull(libro, nameof(libro.Editorial), request.Editorial);
        SetIfNotNull(libro, nameof(libro.AnoPublicacion), request.AnoPublicacion);
        SetIfNotNull(libro, nameof(libro.Genero), request.Genero);
        SetIfNotNull(libro, nameof(libro.CantidadTotal), request.CantidadTotal);

        await _libroRepository.UpdateAsync(libro);

        // Invalidar caché de lista de libros
        await _cacheService.DeleteAsync("libros:lista:*");

        return LibroDto.FromEntity(libro);
    }

    private static void SetIfNotNull<T>(object target, string propertyName, T? value)
    {
        if (value is not null)
        {
            var property = target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            property?.SetValue(target, value);
        }
    }
}