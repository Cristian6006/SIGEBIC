using MediatR;
using SIGEBIC.Application.Common.Exceptions;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Libros.Commands;

public class CreateLibroCommandHandler : IRequestHandler<CreateLibroCommand, LibroDto>
{
    private readonly ILibroRepository _libroRepository;
    private readonly ICacheService _cacheService;

    public CreateLibroCommandHandler(ILibroRepository libroRepository, ICacheService cacheService)
    {
        _libroRepository = libroRepository;
        _cacheService = cacheService;
    }

    public async Task<LibroDto> Handle(CreateLibroCommand request, CancellationToken cancellationToken)
    {
        // Verificar que no exista otro libro con el mismo ISBN
        var existente = await _libroRepository.GetByISBNAsync(request.ISBN);
        if (existente is not null)
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    ["ISBN"] = new[] { "Ya existe un libro con ese ISBN." }
                });

        var libro = new Libro(
            Guid.NewGuid(),
            request.ISBN,
            request.Titulo,
            request.Autor,
            request.Editorial ?? string.Empty,
            request.AnoPublicacion,
            request.Genero ?? string.Empty,
            request.CantidadTotal);

        await _libroRepository.AddAsync(libro);

        // Invalidar caché de lista de libros
        await _cacheService.DeleteAsync("libros:lista:*");

        return LibroDto.FromEntity(libro);
    }
}