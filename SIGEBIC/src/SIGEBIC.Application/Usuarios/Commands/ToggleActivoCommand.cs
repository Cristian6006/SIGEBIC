using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Usuarios.Commands;

public record ToggleActivoCommand(
    Guid UsuarioId,
    bool Activar) : IRequest<UsuarioDto>;