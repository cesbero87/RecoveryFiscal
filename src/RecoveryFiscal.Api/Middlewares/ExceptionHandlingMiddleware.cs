using System.Net;
using Microsoft.AspNetCore.Mvc;
using RecoveryFiscal.Application.Common.Exceptions;

namespace RecoveryFiscal.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (ValidationAppException ex)
        {
            await WriteProblemDetailsAsync(context, HttpStatusCode.UnprocessableEntity, "Erro de validação", ex.Message, ex.Errors);
        }
        catch (ConflictException ex)
        {
            await WriteProblemDetailsAsync(context, HttpStatusCode.Conflict, "Conflito", ex.Message);
        }
        catch (NotFoundException ex)
        {
            await WriteProblemDetailsAsync(context, HttpStatusCode.NotFound, "Recurso não encontrado", ex.Message);
        }
        catch (ArgumentException ex)
        {
            await WriteProblemDetailsAsync(context, HttpStatusCode.BadRequest, "Pedido inválido", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado ao processar o pedido.");
            await WriteProblemDetailsAsync(context, HttpStatusCode.InternalServerError, "Erro interno", "Ocorreu um erro interno no servidor.");
        }
    }

    private static async Task WriteProblemDetailsAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string title,
        string detail,
        IReadOnlyDictionary<string, string[]>? errors = null)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        problem.Extensions["traceId"] = context.TraceIdentifier;

        if (errors is not null)
        {
            problem.Extensions["errors"] = errors;
        }

        await context.Response.WriteAsJsonAsync(problem);
    }
}
