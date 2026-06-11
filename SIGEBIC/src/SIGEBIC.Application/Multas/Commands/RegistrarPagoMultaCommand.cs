using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Multas.Commands;

public record RegistrarPagoMultaCommand(
    Guid MultaId,
    string? Observaciones) : IRequest<MultaDto>;