using System.Net;
using System.Text.Json;
using SIGEBIC.Application.Common.Exceptions;

namespace SIGEBIC.Web.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Recurso no encontrado");
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (UnauthorizedException ex)
        {
            _logger.LogWarning(ex, "Acceso no autorizado");
            await HandleExceptionAsync(context, HttpStatusCode.Unauthorized, ex.Message);
        }
        catch (SIGEBIC.Application.Common.Exceptions.ValidationException ex)
        {
            _logger.LogWarning(ex, "Error de validación");
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error interno del servidor");
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "Ha ocurrido un error interno del servidor.");
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        var response = new
        {
            error = message,
            statusCode = (int)statusCode
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static async Task HandleValidationExceptionAsync(HttpContext context, SIGEBIC.Application.Common.Exceptions.ValidationException ex)
    {
        var response = new
        {
            error = ex.Message,
            statusCode = (int)HttpStatusCode.BadRequest,
            errors = ex.Errors
        };

        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}