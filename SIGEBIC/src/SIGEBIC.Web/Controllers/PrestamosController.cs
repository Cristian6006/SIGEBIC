using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Application.Prestamos.Commands;
using SIGEBIC.Application.Prestamos.Queries;

namespace SIGEBIC.Web.Controllers;

[ApiController]
[Route("api/prestamos")]
public class PrestamosController : ControllerBase
{
    private readonly IMediator _mediator;

    public PrestamosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene un listado paginado de préstamos con filtros opcionales.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    [ProducesResponseType(typeof(PagedResult<PrestamoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPrestamos(
        [FromQuery] Guid? usuarioId,
        [FromQuery] Guid? libroId,
        [FromQuery] string? estado,
        [FromQuery] bool? vencidos,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPrestamosQuery(
            usuarioId,
            libroId,
            estado is not null ? Enum.Parse<Domain.Enums.EstadoPrestamo>(estado) : null,
            vencidos,
            pagina,
            tamanoPagina);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene la lista de préstamos vencidos (panel de alertas).
    /// </summary>
    [HttpGet("vencidos")]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    [ProducesResponseType(typeof(IReadOnlyList<PrestamoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPrestamosVencidos(CancellationToken cancellationToken = default)
    {
        var query = new GetPrestamosVencidosQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene los préstamos del usuario autenticado (Lector).
    /// </summary>
    [HttpGet("mis-prestamos")]
    [Authorize]
    [ProducesResponseType(typeof(PagedResult<PrestamoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMisPrestamos(
        [FromQuery] string? estado,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 10,
        CancellationToken cancellationToken = default)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            ?? User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var usuarioId))
            return Unauthorized(new { error = "Token inválido: no se pudo extraer el usuario." });

        var query = new GetMisPrestamosQuery(
            usuarioId,
            estado is not null ? Enum.Parse<Domain.Enums.EstadoPrestamo>(estado) : null,
            pagina,
            tamanoPagina);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene un préstamo por su Id.
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    [ProducesResponseType(typeof(PrestamoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPrestamoById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPrestamoByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Registra un nuevo préstamo.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    [ProducesResponseType(typeof(PrestamoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RegistrarPrestamo(
        [FromBody] RegistrarPrestamoCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetPrestamoById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Registra la devolución de un préstamo.
    /// </summary>
    [HttpPost("{id:guid}/devolver")]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    [ProducesResponseType(typeof(PrestamoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DevolverPrestamo(
        [FromRoute] Guid id,
        [FromBody] DevolverPrestamoRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new RegistrarDevolucionCommand(id, request.Observaciones);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Renueva un préstamo existente.
    /// </summary>
    [HttpPost("{id:guid}/renovar")]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    [ProducesResponseType(typeof(PrestamoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RenovarPrestamo(
        [FromRoute] Guid id,
        [FromBody] RenovarPrestamoRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new RenovarPrestamoCommand(id, request.DiasExtension);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}

/// <summary>
/// DTO para la solicitud de devolución.
/// </summary>
public record DevolverPrestamoRequest(string? Observaciones);

/// <summary>
/// DTO para la solicitud de renovación.
/// </summary>
public record RenovarPrestamoRequest(int DiasExtension = 7);