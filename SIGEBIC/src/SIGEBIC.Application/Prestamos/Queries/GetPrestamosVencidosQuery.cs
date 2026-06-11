using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Prestamos.Queries;

public record GetPrestamosVencidosQuery : IRequest<IReadOnlyList<PrestamoDto>>;