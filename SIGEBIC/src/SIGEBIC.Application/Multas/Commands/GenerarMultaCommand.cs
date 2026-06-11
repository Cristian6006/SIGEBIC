using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Multas.Commands;

public record GenerarMultaCommand(
    Guid PrestamoId,
    int DiasRetraso) : IRequest<MultaDto?>;