using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Usuarios.Queries;

public record GetUsuariosQuery(
    string? Nombre,
    string? Email,
    Guid? RolId,
    bool? Activo,
    int Pagina = 1,
    int TamanoPagina = 15) : IRequest<PagedResult<UsuarioDto>>;