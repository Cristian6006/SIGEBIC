using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Application.Historial.Queries;

namespace SIGEBIC.Web.Controllers;

[ApiController]
[Route("api/historial")]
public class HistorialController : ControllerBase
{
    private readonly IMediator _mediator;

    public HistorialController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene el historial de préstamos de un libro específico.
    /// </summary>
    [HttpGet("libros/{libroId:guid}")]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    [ProducesResponseType(typeof(PagedResult<HistorialPrestamoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetHistorialByLibro(
        [FromRoute] Guid libroId,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetHistorialByLibroQuery(libroId, pagina, tamanoPagina);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene el historial de préstamos de un usuario específico.
    /// Los Lectores solo pueden ver su propio historial.
    /// </summary>
    [HttpGet("usuarios/{usuarioId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(PagedResult<HistorialPrestamoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetHistorialByUsuario(
        [FromRoute] Guid usuarioId,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 10,
        CancellationToken cancellationToken = default)
    {
        // Si el rol es Lector, verificar que el usuarioId coincida con el JWT
        if (User.IsInRole("Lector"))
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId) || userId != usuarioId)
                return Forbid();
        }

        var query = new GetHistorialByUsuarioQuery(usuarioId, pagina, tamanoPagina);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene el historial de préstamos del usuario autenticado.
    /// </summary>
    [HttpGet("mi-historial")]
    [Authorize]
    [ProducesResponseType(typeof(PagedResult<HistorialPrestamoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMiHistorial(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 10,
        CancellationToken cancellationToken = default)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            ?? User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var usuarioId))
            return Unauthorized(new { error = "Token inválido: no se pudo extraer el usuario." });

        var query = new GetHistorialByUsuarioQuery(usuarioId, pagina, tamanoPagina);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}