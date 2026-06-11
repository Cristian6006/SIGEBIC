using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Application.Libros.Commands;
using SIGEBIC.Application.Libros.Queries;

namespace SIGEBIC.Web.Controllers;

[ApiController]
[Route("api/libros")]
public class LibrosController : ControllerBase
{
    private readonly IMediator _mediator;

    public LibrosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene un listado paginado de libros con filtros opcionales.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResult<LibroDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLibros(
        [FromQuery] string? titulo,
        [FromQuery] string? autor,
        [FromQuery] string? genero,
        [FromQuery] bool? soloDisponibles,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetLibrosQuery(titulo, autor, genero, soloDisponibles, pagina, tamanoPagina);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene un libro por su Id.
    /// </summary>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LibroDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLibroById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetLibroByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene un libro por su ISBN.
    /// </summary>
    [HttpGet("isbn/{isbn}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LibroDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLibroByISBN(
        [FromRoute] string isbn,
        CancellationToken cancellationToken = default)
    {
        var query = new GetLibroByISBNQuery(isbn);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Crea un nuevo libro en el catálogo.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    [ProducesResponseType(typeof(LibroDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateLibro(
        [FromBody] CreateLibroCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetLibroById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Actualiza los datos de un libro existente.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    [ProducesResponseType(typeof(LibroDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLibro(
        [FromRoute] Guid id,
        [FromBody] UpdateLibroRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateLibroCommand(
            id,
            request.ISBN,
            request.Titulo,
            request.Autor,
            request.Editorial,
            request.AnoPublicacion,
            request.Genero,
            request.CantidadTotal);

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Da de baja un libro (solo Administrador).
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DarDeBajaLibro(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new DarDeBajaLibroCommand(id);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}

/// <summary>
/// DTO para la solicitud de actualización de libro (sin el Id, que va en la ruta).
/// </summary>
public record UpdateLibroRequest(
    string? ISBN,
    string? Titulo,
    string? Autor,
    string? Editorial,
    int? AnoPublicacion,
    string? Genero,
    int? CantidadTotal);