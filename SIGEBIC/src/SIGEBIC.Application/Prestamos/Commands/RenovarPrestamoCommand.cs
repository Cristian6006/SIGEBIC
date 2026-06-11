using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Prestamos.Commands;

public record RenovarPrestamoCommand(
    Guid PrestamoId,
    int DiasExtension = 7) : IRequest<PrestamoDto>;