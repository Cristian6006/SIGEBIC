using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Usuarios.Queries;

public record GetRolesQuery : IRequest<IReadOnlyList<RolDto>>;