using MediatR;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Usuarios.Queries;

public class GetUsuariosQueryHandler : IRequestHandler<GetUsuariosQuery, PagedResult<UsuarioDto>>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public GetUsuariosQueryHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<PagedResult<UsuarioDto>> Handle(GetUsuariosQuery request, CancellationToken cancellationToken)
    {
        var spec = new UsuarioQuerySpecification(
            request.Nombre,
            request.Email,
            request.RolId,
            request.Activo,
            request.Pagina,
            request.TamanoPagina);

        // Ejecutar ambas consultas secuencialmente (DbContext no es thread-safe)
        var usuarios = await _usuarioRepository.GetAllAsync(spec);
        var totalRegistros = await _usuarioRepository.GetCountAsync(spec);

        var items = usuarios
            .Select(UsuarioDto.FromEntity)
            .ToList();

        return new PagedResult<UsuarioDto>(
            items,
            request.Pagina,
            request.TamanoPagina,
            totalRegistros);
    }

    private sealed class UsuarioQuerySpecification : IUsuarioSpecification
    {
        public string? Nombre { get; }
        public string? Email { get; }
        public Guid? RolId { get; }
        public bool? Activo { get; }
        public int Pagina { get; }
        public int TamanoPagina { get; }

        public UsuarioQuerySpecification(
            string? nombre,
            string? email,
            Guid? rolId,
            bool? activo,
            int pagina,
            int tamanoPagina)
        {
            Nombre = nombre;
            Email = email;
            RolId = rolId;
            Activo = activo;
            Pagina = pagina;
            TamanoPagina = tamanoPagina;
        }
    }
}