using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBIC.Application.Auth.Commands;
using SIGEBIC.Application.Common.Exceptions;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Web.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Inicia sesión con credenciales de usuario.
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var command = new LoginCommand(request.Email, request.Password);
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }
        catch (UnauthorizedException)
        {
            return Unauthorized(new { error = "Credenciales inválidas." });
        }
    }

    /// <summary>
    /// Cierra sesión y revoca el token JWT actual.
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        // Extraer el token del header Authorization
        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader is null || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new { error = "Token no proporcionado en el header Authorization." });

        var token = authHeader["Bearer ".Length..].Trim();

        // Extraer el UserId del claim 'sub' (JWT subject)
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            ?? User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { error = "Token inválido: no se pudo extraer el usuario." });

        var command = new LogoutCommand(userId, token);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}

public record LoginRequest(string Email, string Password);