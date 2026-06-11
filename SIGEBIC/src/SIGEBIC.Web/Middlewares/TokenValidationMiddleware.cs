using System.IdentityModel.Tokens.Jwt;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Web.Middlewares;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICacheService cacheService)
    {
        // Obtener el token del header Authorization
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (authHeader is not null && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var token = authHeader["Bearer ".Length..].Trim();

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    if (handler.CanReadToken(token))
                    {
                        var jwtToken = handler.ReadJwtToken(token);
                        var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                        if (!string.IsNullOrEmpty(jti))
                        {
                            var cacheKey = $"token:{jti}";
                            var tokenEnCache = await cacheService.GetAsync(cacheKey);

                            // Si el token no existe en Redis, está revocado o expirado
                            if (tokenEnCache is null)
                            {
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                context.Response.ContentType = "application/json";
                                await context.Response.WriteAsync("{\"error\":\"Token revocado o expirado.\"}");
                                return;
                            }
                        }
                    }
                }
                catch
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{\"error\":\"Token inválido.\"}");
                    return;
                }
            }
        }

        await _next(context);
    }
}