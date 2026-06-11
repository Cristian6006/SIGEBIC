using MediatR;
using SIGEBIC.Application.Common.Exceptions;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Events;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Prestamos.Commands;

public class RegistrarPrestamoCommandHandler : IRequestHandler<RegistrarPrestamoCommand, PrestamoDto>
{
    private readonly IPrestamoRepository _prestamoRepository;
    private readonly ILibroRepository _libroRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMultaRepository _multaRepository;
    private readonly IPublisher _publisher;
    private readonly IUnitOfWork _unitOfWork;

    public RegistrarPrestamoCommandHandler(
        IPrestamoRepository prestamoRepository,
        ILibroRepository libroRepository,
        IUsuarioRepository usuarioRepository,
        IMultaRepository multaRepository,
        IPublisher publisher,
        IUnitOfWork unitOfWork)
    {
        _prestamoRepository = prestamoRepository;
        _libroRepository = libroRepository;
        _usuarioRepository = usuarioRepository;
        _multaRepository = multaRepository;
        _publisher = publisher;
        _unitOfWork = unitOfWork;
    }

    public async Task<PrestamoDto> Handle(RegistrarPrestamoCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar usuario
        var usuario = await _usuarioRepository.GetByIdAsync(request.UsuarioId);
        if (usuario is null)
            throw new NotFoundException(nameof(Usuario), request.UsuarioId);

        if (!usuario.Activo)
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    ["UsuarioId"] = new[] { "El usuario está inactivo." }
                });

        // 2. Validar libro
        var libro = await _libroRepository.GetByIdAsync(request.LibroId);
        if (libro is null)
            throw new NotFoundException(nameof(Libro), request.LibroId);

        if (!libro.EstaDisponible())
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    ["LibroId"] = new[] { "El libro no tiene ejemplares disponibles." }
                });

        // 3. Validar que no exista préstamo activo del mismo usuario/libro
        var prestamoActivo = await _prestamoRepository.GetActivoByUsuarioYLibroAsync(
            request.UsuarioId, request.LibroId);
        if (prestamoActivo is not null)
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    ["LibroId"] = new[] { "El usuario ya tiene este libro prestado." }
                });

        // 3.5 Validar que el usuario no tenga multas pendientes
        var tieneMultaPendiente = await _multaRepository.TieneMultaPendienteAsync(request.UsuarioId);
        if (tieneMultaPendiente)
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    ["UsuarioId"] = new[] { "El usuario tiene multas pendientes. Debe pagarlas antes de realizar un nuevo préstamo." }
                });

        // 4. Crear la entidad Prestamo
        var prestamo = new Prestamo(
            Guid.NewGuid(),
            request.UsuarioId,
            request.LibroId,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(request.DiasPrestamo),
            request.Observaciones);

        // 5. Descontar ejemplar del libro
        libro.DescontarEjemplar();

        // 6. Persistir todo dentro de una transacción explícita
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _prestamoRepository.AddAsync(prestamo);
            await _libroRepository.UpdateAsync(libro);

            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }

        // 7. Publicar evento de notificación (fuera de la transacción)
        await _publisher.Publish(new PrestamoRegistradoEvent(
            prestamo.Id,
            usuario.Id,
            usuario.Email,
            libro.Titulo,
            prestamo.FechaDevolucionEsperada), cancellationToken);

        // 8. Retornar DTO
        return PrestamoDto.FromEntity(prestamo);
    }
}