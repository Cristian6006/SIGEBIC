using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Usuarios.Queries;

public record GetUsuarioByIdQuery(Guid Id) : IRequest<UsuarioDto>;