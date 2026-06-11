using MediatR;
using SIGEBIC.Application.Common.Exceptions;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Enums;
using SIGEBIC.Domain.Events;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Prestamos.Commands;

public class RegistrarDevolucionCommandHandler : IRequestHandler<RegistrarDevolucionCommand, PrestamoDto>
{
    private readonly IPrestamoRepository _prestamoRepository;
    private readonly ILibroRepository _libroRepository;
    private readonly IHistorialPrestamoRepository _historialPrestamoRepository;
    private readonly IPublisher _publisher;
    private readonly IUnitOfWork _unitOfWork;

    public RegistrarDevolucionCommandHandler(
        IPrestamoRepository prestamoRepository,
        ILibroRepository libroRepository,
        IHistorialPrestamoRepository historialPrestamoRepository,
        IPublisher publisher,
        IUnitOfWork unitOfWork)
    {
        _prestamoRepository = prestamoRepository;
        _libroRepository = libroRepository;
        _historialPrestamoRepository = historialPrestamoRepository;
        _publisher = publisher;
        _unitOfWork = unitOfWork;
    }

    public async Task<PrestamoDto> Handle(RegistrarDevolucionCommand request, CancellationToken cancellationToken)
    {
        // 1. Buscar el préstamo
        var prestamo = await _prestamoRepository.GetByIdAsync(request.PrestamoId);
        if (prestamo is null)
            throw new NotFoundException(nameof(Prestamo), request.PrestamoId);

        if (prestamo.Estado != EstadoPrestamo.Activo && prestamo.Estado != EstadoPrestamo.Renovado)
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    ["PrestamoId"] = new[] { "Este préstamo ya fue procesado." }
                });

        // 2. Registrar la devolución
        var fechaReal = DateTime.UtcNow;
        prestamo.Devolver(fechaReal);
        var diasRetraso = prestamo.CalcularDiasRetraso();

        // 3. Devolver ejemplar del libro
        prestamo.Libro.DevolverEjemplar();

        // 4. Crear historial del préstamo
        var historial = HistorialPrestamo.CrearDesdePrestamo(prestamo);

        // 5. Persistir en transacción
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _prestamoRepository.UpdateAsync(prestamo);
            await _libroRepository.UpdateAsync(prestamo.Libro);
            await _historialPrestamoRepository.AddAsync(historial);

            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }

        // 6. Publicar evento de devolución después del commit
        await _publisher.Publish(new PrestamoDevueltoEvent(
            prestamo.Id,
            prestamo.LibroId,
            prestamo.UsuarioId,
            diasRetraso,
            fechaReal,
            request.Observaciones ?? prestamo.Observaciones), cancellationToken);

        // 7. Retornar DTO
        return PrestamoDto.FromEntity(prestamo);
    }
}