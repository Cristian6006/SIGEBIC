using MediatR;
using SIGEBIC.Application.Common.Exceptions;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Usuarios.Queries;

public class GetUsuarioByIdQueryHandler : IRequestHandler<GetUsuarioByIdQuery, UsuarioDto>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public GetUsuarioByIdQueryHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<UsuarioDto> Handle(GetUsuarioByIdQuery request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(request.Id);
        if (usuario is null)
            throw new NotFoundException(nameof(Domain.Entities.Usuario), request.Id);

        return UsuarioDto.FromEntity(usuario);
    }
}