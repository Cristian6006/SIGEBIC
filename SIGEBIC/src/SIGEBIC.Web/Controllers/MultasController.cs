using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Application.Multas.Commands;
using SIGEBIC.Application.Multas.Queries;

namespace SIGEBIC.Web.Controllers;

[ApiController]
[Route("api/multas")]
public class MultasController : ControllerBase
{
    private readonly IMediator _mediator;

    public MultasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todas las multas pendientes (panel de alertas del Bibliotecario).
    /// </summary>
    [HttpGet("pendientes")]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    [ProducesResponseType(typeof(IReadOnlyList<MultaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetMultasPendientes(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetMultasPendientesQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene las multas de un usuario específico (Administrador/Bibliotecario).
    /// </summary>
    [HttpGet("usuario/{usuarioId:guid}")]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    [ProducesResponseType(typeof(PagedResult<MultaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetMultasByUsuario(
        [FromRoute] Guid usuarioId,
        [FromQuery] bool? soloPendientes,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMultasByUsuarioQuery(usuarioId, soloPendientes, pagina, tamanoPagina);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene las multas del usuario autenticado (Lector).
    /// </summary>
    [HttpGet("mis-multas")]
    [Authorize]
    [ProducesResponseType(typeof(PagedResult<MultaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMisMultas(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 10,
        CancellationToken cancellationToken = default)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            ?? User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var usuarioId))
            return Unauthorized(new { error = "Token inválido: no se pudo extraer el usuario." });

        var query = new GetMisMultasQuery(usuarioId, pagina, tamanoPagina);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene una multa por su Id.
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    [ProducesResponseType(typeof(MultaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMultaById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMultaByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Registra el pago de una multa.
    /// </summary>
    [HttpPost("{id:guid}/pagar")]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    [ProducesResponseType(typeof(MultaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PagarMulta(
        [FromRoute] Guid id,
        [FromBody] PagarMultaRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new RegistrarPagoMultaCommand(id, request.Observaciones);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}

/// <summary>
/// DTO para la solicitud de pago de multa.
/// </summary>
public record PagarMultaRequest(string? Observaciones);