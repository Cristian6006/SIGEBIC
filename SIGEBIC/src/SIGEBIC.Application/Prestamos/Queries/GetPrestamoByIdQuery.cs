using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Prestamos.Queries;

public record GetPrestamoByIdQuery(Guid Id) : IRequest<PrestamoDto>;