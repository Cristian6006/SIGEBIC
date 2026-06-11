using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Usuarios.Commands;

public record UpdateUsuarioCommand(
    Guid Id,
    string? Nombre,
    string? Apellido,
    string? Telefono,
    string? NumeroDocumento) : IRequest<UsuarioDto>;