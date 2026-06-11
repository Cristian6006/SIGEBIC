using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Prestamos.Commands;

public record RegistrarPrestamoCommand(
    Guid UsuarioId,
    Guid LibroId,
    int DiasPrestamo = 14,
    string? Observaciones = null) : IRequest<PrestamoDto>;
