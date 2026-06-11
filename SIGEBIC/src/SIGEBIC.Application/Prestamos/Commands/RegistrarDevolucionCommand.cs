using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Prestamos.Commands;

public record RegistrarDevolucionCommand(
    Guid PrestamoId,
    string? Observaciones) : IRequest<PrestamoDto>;