using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SIGEBIC.Domain.Entities;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Infrastructure.Auth;

public class JwtTokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly ICacheService _cacheService;

    public JwtTokenService(IConfiguration configuration, ICacheService cacheService)
    {
        _configuration = configuration;
        _cacheService = cacheService;
    }

    public string GenerarToken(Usuario usuario)
    {
        var secretKey = _configuration["Jwt:SecretKey"]
            ?? throw new InvalidOperationException("JWT SecretKey no está configurada.");
        var issuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("JWT Issuer no está configurado.");
        var audience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("JWT Audience no está configurada.");
        var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");

        var jti = Guid.NewGuid().ToString();
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol.Nombre.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, jti),
            new Claim("nombreCompleto", $"{usuario.Nombre} {usuario.Apellido}")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        // Guardar el token en Redis con TTL igual a ExpirationMinutes
        var cacheKey = $"token:{jti}";
        _cacheService.SetAsync(cacheKey, tokenString, TimeSpan.FromMinutes(expirationMinutes)).GetAwaiter().GetResult();

        return tokenString;
    }

    public async Task RevocarToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        if (handler.CanReadToken(token))
        {
            var jwtToken = handler.ReadJwtToken(token);
            var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            if (!string.IsNullOrEmpty(jti))
            {
                var cacheKey = $"token:{jti}";
                await _cacheService.DeleteAsync(cacheKey);
            }
        }
    }
}