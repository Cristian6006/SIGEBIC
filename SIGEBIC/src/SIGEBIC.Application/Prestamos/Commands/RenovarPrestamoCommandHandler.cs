using MediatR;
using SIGEBIC.Application.Common.Exceptions;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Prestamos.Commands;

public class RenovarPrestamoCommandHandler : IRequestHandler<RenovarPrestamoCommand, PrestamoDto>
{
    private readonly IPrestamoRepository _prestamoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RenovarPrestamoCommandHandler(
        IPrestamoRepository prestamoRepository,
        IUnitOfWork unitOfWork)
    {
        _prestamoRepository = prestamoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PrestamoDto> Handle(RenovarPrestamoCommand request, CancellationToken cancellationToken)
    {
        // 1. Buscar el préstamo
        var prestamo = await _prestamoRepository.GetByIdAsync(request.PrestamoId);
        if (prestamo is null)
            throw new NotFoundException(nameof(Prestamo), request.PrestamoId);

        // 2. Renovar — el método de dominio lanza InvalidOperationException si no es válido
        prestamo.Renovar(request.DiasExtension);

        // 3. Persistir en transacción
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _prestamoRepository.UpdateAsync(prestamo);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }

        // 4. Retornar DTO
        return PrestamoDto.FromEntity(prestamo);
    }
}