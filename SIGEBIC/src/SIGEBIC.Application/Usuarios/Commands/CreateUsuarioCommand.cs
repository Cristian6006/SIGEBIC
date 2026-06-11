using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Usuarios.Commands;

public record CreateUsuarioCommand(
    string Nombre,
    string Apellido,
    string Email,
    string Password,
    string? Telefono,
    string NumeroDocumento,
    Guid RolId) : IRequest<UsuarioDto>;