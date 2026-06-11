using MediatR;
using SIGEBIC.Application.Common.Exceptions;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IUsuarioRepository usuarioRepository, ITokenService tokenService)
    {
        _usuarioRepository = usuarioRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Buscar usuario por email
        var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);

        // 2. Validar existencia y contraseña
        if (usuario is null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
            throw new UnauthorizedException("Credenciales inválidas.");

        // 3. Validar que el usuario esté activo
        if (!usuario.Activo)
            throw new UnauthorizedException("La cuenta de usuario está inactiva. Contacte al administrador.");

        // 4. Generar JWT
        var token = _tokenService.GenerarToken(usuario);
        var expiracion = DateTime.UtcNow.AddMinutes(60);

        return new LoginResponse(
            Token: token,
            Email: usuario.Email,
            NombreCompleto: $"{usuario.Nombre} {usuario.Apellido}",
            Rol: usuario.Rol.Nombre.ToString(),
            Expiracion: expiracion);
    }
}
