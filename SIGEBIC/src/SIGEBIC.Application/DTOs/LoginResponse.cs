namespace SIGEBIC.Application.DTOs;

public record LoginResponse(
    string Token,
    string Email,
    string NombreCompleto,
    string Rol,
    DateTime Expiracion);
