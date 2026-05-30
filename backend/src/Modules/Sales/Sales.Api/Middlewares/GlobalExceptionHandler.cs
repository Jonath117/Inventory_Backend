using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Sales.Api.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Excepción no controlada capturada: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        switch (exception)
        {
            case KeyNotFoundException keyNotFoundEx:
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                problemDetails.Title = "Recurso no encontrado";
                problemDetails.Detail = keyNotFoundEx.Message;
                problemDetails.Status = StatusCodes.Status404NotFound;
                break;

            case ArgumentException argEx:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Argumento invalido";
                problemDetails.Detail = argEx.Message;
                problemDetails.Status = StatusCodes.Status400BadRequest;
                break;

            case InvalidOperationException invEx:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Operacion invalida";
                problemDetails.Detail = invEx.Message;
                problemDetails.Status = StatusCodes.Status400BadRequest;
                break;

            case UnauthorizedAccessException unauthEx:
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                problemDetails.Title = "Acceso denegado";
                problemDetails.Detail = unauthEx.Message;
                problemDetails.Status = StatusCodes.Status403Forbidden;
                break;
            
            case HttpRequestException httpEx:
            case TaskCanceledException taskEx:
                httpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                problemDetails.Title = "Servicio no disponible";
                problemDetails.Detail = "No se pudo establecer conexión con el sistema de inventario. Verifica que el servicio esté en línea.";
                problemDetails.Status = StatusCodes.Status503ServiceUnavailable;
                break;

            default:
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "Error interno del servidor";
                problemDetails.Detail = "Ocurrio un error inesperado. Intenta nuevamente o contacta a soporte.";
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                break;
        }

        httpContext.Response.ContentType = "application/problem+json";
        
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true; 
    }
}