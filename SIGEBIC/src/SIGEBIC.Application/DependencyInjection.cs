using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SIGEBIC.Application.Common.Behaviors;

namespace SIGEBIC.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        // MediatR: registra todos los handlers del ensamblado Application
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);

            // Agrega el pipeline de validación
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        // FluentValidation: registra todos los validators del ensamblado Application
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
