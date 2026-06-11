using MediatR;
using SIGEBIC.Application.Common.Exceptions;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Prestamos.Queries;

public class GetPrestamoByIdQueryHandler : IRequestHandler<GetPrestamoByIdQuery, PrestamoDto>
{
    private readonly IPrestamoRepository _prestamoRepository;

    public GetPrestamoByIdQueryHandler(IPrestamoRepository prestamoRepository)
    {
        _prestamoRepository = prestamoRepository;
    }

    public async Task<PrestamoDto> Handle(GetPrestamoByIdQuery request, CancellationToken cancellationToken)
    {
        var prestamo = await _prestamoRepository.GetByIdAsync(request.Id);
        if (prestamo is null)
            throw new NotFoundException(nameof(Prestamo), request.Id);

        return PrestamoDto.FromEntity(prestamo);
    }
}