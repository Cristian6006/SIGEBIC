using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Usuarios.Commands;

public record AsignarRolCommand(
    Guid UsuarioId,
    Guid RolId) : IRequest<UsuarioDto>;