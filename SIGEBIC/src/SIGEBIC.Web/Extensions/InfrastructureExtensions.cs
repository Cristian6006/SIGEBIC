using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SIGEBIC.Domain.Interfaces;
using SIGEBIC.Infrastructure.Auth;
using SIGEBIC.Infrastructure.Cache;
using SIGEBIC.Infrastructure.Repositories;

namespace SIGEBIC.Web.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres")));

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ILibroRepository, LibroRepository>();

        // Cache
        services.AddSingleton<ICacheService, RedisCacheService>();

        // JWT Token Service
        services.AddScoped<ITokenService, JwtTokenService>();

        // JWT Authentication
        var jwtSection = configuration.GetSection("Jwt");
        var secretKey = jwtSection["SecretKey"]
            ?? throw new InvalidOperationException("JWT SecretKey no está configurada.");
        var issuer = jwtSection["Issuer"]
            ?? throw new InvalidOperationException("JWT Issuer no está configurado.");
        var audience = jwtSection["Audience"]
            ?? throw new InvalidOperationException("JWT Audience no está configurada.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }
}
