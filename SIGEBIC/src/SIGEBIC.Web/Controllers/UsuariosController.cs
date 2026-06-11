using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Application.Usuarios.Commands;
using SIGEBIC.Application.Usuarios.Queries;

namespace SIGEBIC.Web.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsuariosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene un listado paginado de usuarios con filtros opcionales.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(PagedResult<UsuarioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetUsuarios(
        [FromQuery] string? nombre,
        [FromQuery] string? email,
        [FromQuery] Guid? rolId,
        [FromQuery] bool? activo,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 15,
        CancellationToken cancellationToken = default)
    {
        var query = new GetUsuariosQuery(nombre, email, rolId, activo, pagina, tamanoPagina);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene un usuario por su Id.
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUsuarioById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetUsuarioByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene el perfil del usuario autenticado.
    /// </summary>
    [HttpGet("perfil")]
    [Authorize]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetPerfil(CancellationToken cancellationToken = default)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            ?? User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { error = "Token inválido: no se pudo extraer el usuario." });

        var query = new GetUsuarioByIdQuery(userId);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Crea un nuevo usuario.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateUsuario(
        [FromBody] CreateUsuarioCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetUsuarioById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Actualiza los datos de un usuario existente.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUsuario(
        [FromRoute] Guid id,
        [FromBody] UpdateUsuarioRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateUsuarioCommand(
            id,
            request.Nombre,
            request.Apellido,
            request.Telefono,
            request.NumeroDocumento);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Cambia la contraseña de un usuario.
    /// </summary>
    [HttpPatch("{id:guid}/password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CambiarPassword(
        [FromRoute] Guid id,
        [FromBody] CambiarPasswordRequest request,
        CancellationToken cancellationToken = default)
    {
        // Verificar que el usuario autenticado sea el propietario o un Administrador
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            ?? User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        var esAdmin = User.IsInRole("Administrador");

        if (!esAdmin)
        {
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId) || userId != id)
                return Unauthorized(new { error = "No tienes permiso para cambiar esta contraseña." });
        }

        var command = new CambiarPasswordCommand(id, request.PasswordActual, request.NuevoPassword);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Activa o desactiva un usuario.
    /// </summary>
    [HttpPatch("{id:guid}/activo")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleActivo(
        [FromRoute] Guid id,
        [FromBody] ToggleActivoRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new ToggleActivoCommand(id, request.Activar);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Cambia el rol de un usuario.
    /// </summary>
    [HttpPatch("{id:guid}/rol")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AsignarRol(
        [FromRoute] Guid id,
        [FromBody] AsignarRolRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new AsignarRolCommand(id, request.RolId);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}

/// <summary>
/// DTO para la solicitud de actualización de usuario (sin el Id, que va en la ruta).
/// </summary>
public record UpdateUsuarioRequest(
    string? Nombre,
    string? Apellido,
    string? Telefono,
    string? NumeroDocumento);

/// <summary>
/// DTO para la solicitud de cambio de contraseña.
/// </summary>
public record CambiarPasswordRequest(
    string PasswordActual,
    string NuevoPassword);

/// <summary>
/// DTO para la solicitud de activar/desactivar usuario.
/// </summary>
public record ToggleActivoRequest(bool Activar);

/// <summary>
/// DTO para la solicitud de asignación de rol.
/// </summary>
public record AsignarRolRequest(Guid RolId);