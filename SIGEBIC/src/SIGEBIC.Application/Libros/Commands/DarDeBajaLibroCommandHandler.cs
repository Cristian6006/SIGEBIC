using MediatR;
using SIGEBIC.Application.Common.Exceptions;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Libros.Commands;

public class DarDeBajaLibroCommandHandler : IRequestHandler<DarDeBajaLibroCommand>
{
    private readonly ILibroRepository _libroRepository;

    public DarDeBajaLibroCommandHandler(ILibroRepository libroRepository)
    {
        _libroRepository = libroRepository;
    }

    public async Task Handle(DarDeBajaLibroCommand request, CancellationToken cancellationToken)
    {
        var libro = await _libroRepository.GetByIdAsync(request.Id);
        if (libro is null)
            throw new NotFoundException("Libro", request.Id);

        // Validar que no tenga ejemplares prestados
        if (libro.CantidadDisponible < libro.CantidadTotal)
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    ["Libro"] = new[] { "No se puede dar de baja un libro con ejemplares prestados." }
                });

        libro.DarDeBaja();
        await _libroRepository.UpdateAsync(libro);
    }
}