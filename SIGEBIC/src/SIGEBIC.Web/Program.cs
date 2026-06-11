using Microsoft.EntityFrameworkCore;
using MediatR;
using Hangfire;
using Hangfire.PostgreSql;
using StackExchange.Redis;
using FluentValidation;
using Microsoft.OpenApi.Models;
using SIGEBIC.Application;
using SIGEBIC.Infrastructure.Persistence;
using SIGEBIC.Web.Extensions;
using SIGEBIC.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Infraestructura (Base de datos, Repositorios, Redis, JWT y Autenticación)
builder.Services.AddInfrastructure(builder.Configuration);

// Redis (singleton compartido con la infraestructura)
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));

// Application (MediatR + FluentValidation + ValidationBehavior)
builder.Services.AddApplication();

// Hangfire
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(options =>
        options.UseNpgsqlConnection(builder.Configuration.GetConnectionString("Postgres"))));
builder.Services.AddHangfireServer();

builder.Services.AddAuthorization();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT en el formato: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Aplicar migraciones automáticamente al iniciar y seed de datos
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    await DbSeeder.SeedAsync(db);
}

app.UseSwagger();
app.UseSwaggerUI();

// Middleware global de manejo de excepciones (primero)
app.UseMiddleware<GlobalExceptionMiddleware>();

// Middleware de validación de token en Redis (antes de UseAuthentication)
app.UseMiddleware<TokenValidationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHangfireDashboard("/hangfire");

app.Run();

public abstract class ApplicationAssemblyMarker { }
