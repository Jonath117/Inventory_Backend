using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sales.Domain.Exceptions;

namespace Sales.Api.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Excepcion capturada : {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path,
            Extensions = new Dictionary<string, object?>
            {
                { "traceId", httpContext.TraceIdentifier },
            }
        };

        switch (exception)
        {
            case NotFoundException notFoundEx:
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                problemDetails.Title = "Recurso no encontrado";
                problemDetails.Detail = notFoundEx.Message;
                problemDetails.Status = StatusCodes.Status404NotFound;
                break;
            
            case BadRequestException badRequestEx:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Solicitud incorrecta o regla de negocio violada";
                problemDetails.Detail = badRequestEx.Message;
                problemDetails.Status = StatusCodes.Status400BadRequest;
                break;
            
            case InvalidOperationException invEx:
            case ArgumentException argEx:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Operación de dominio inválida";
                problemDetails.Detail = exception.Message;
                problemDetails.Status = StatusCodes.Status400BadRequest;
                break;
            
            default:
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "Error interno del servidor";
                problemDetails.Detail = "Ocurrio un error inesperado al procesar la solicitud en el servidor de Ventas.";
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                break;
        }
        
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}