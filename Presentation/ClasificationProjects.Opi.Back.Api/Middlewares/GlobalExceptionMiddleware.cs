using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using ClasificationProjects.Opi.Back.Api.Resources;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace ClasificationProjects.Opi.Back.Api.Middlewares;

public sealed class GlobalExceptionMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionMiddleware(
        ILogger<GlobalExceptionMiddleware> logger,
        IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "ValidationException. TraceId={TraceId}", GetTraceId(context));

            var errors = ex.Errors
                .Select(e => new ApiError(
                    string.IsNullOrWhiteSpace(e.ErrorCode) ? "VALIDATION_ERROR" : e.ErrorCode,
                    Get("Error.Invalid"),
                    string.IsNullOrWhiteSpace(e.PropertyName) ? null : e.PropertyName
                ))
                .ToList();

            await WriteWrapped(
                context,
                StatusCodes.Status422UnprocessableEntity,
                Get("Title.Validation"),
                Get("Error.FieldsInvalidOrMissing"),
                errors
            );
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "ArgumentException. TraceId={TraceId}", GetTraceId(context));

            await WriteWrapped(
                context,
                StatusCodes.Status400BadRequest,
                Get("Title.BadRequest"),
                _env.IsDevelopment() ? ex.Message : Get("Error.FieldsInvalidOrMissing")
            );
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "KeyNotFoundException. TraceId={TraceId}", GetTraceId(context));

            await WriteWrapped(
                context,
                StatusCodes.Status404NotFound,
                Get("Title.NotFound"),
                _env.IsDevelopment() ? ex.Message : Get("Error.Invalid")
            );
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "InvalidOperationException. TraceId={TraceId}", GetTraceId(context));

            await WriteWrapped(
                context,
                StatusCodes.Status409Conflict,
                Get("Title.Conflict"),
                _env.IsDevelopment() ? ex.ToString() : Get("Error.Invalid")
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception. TraceId={TraceId}", GetTraceId(context));

            if (TryGetPostgresException(ex, out var pg) && pg.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                await WriteWrapped(
                    context,
                    StatusCodes.Status409Conflict,
                    Get("Title.Conflict"),
                    Get("Error.UniqueViolation")
                );
                return;
            }

            await WriteWrapped(
                context,
                StatusCodes.Status500InternalServerError,
                Get("Title.Unexpected"),
                _env.IsDevelopment() ? ex.ToString() : Get("Error.Unexpected")
            );
        }
    }

    private static string Get(string key)
        => ApiMessages.ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? key;

    private static string GetTraceId(HttpContext context)
        => Activity.Current?.Id ?? context.TraceIdentifier;

    private static async Task WriteWrapped(
        HttpContext context,
        int status,
        string title,
        string data,
        IReadOnlyList<ApiError>? errors = null)
    {
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json";

        var traceId = GetTraceId(context);

        var body = new ApiResponse(
            status,
            title,
            data,
            traceId,
            errors ?? Array.Empty<ApiError>());

        await context.Response.WriteAsync(JsonSerializer.Serialize(body));
    }

    private static bool TryGetPostgresException(Exception ex, out PostgresException postgres)
    {
        postgres = null!;

        if (ex is PostgresException pg1)
        {
            postgres = pg1;
            return true;
        }

        if (ex is DbUpdateException dbu && dbu.InnerException is PostgresException pg2)
        {
            postgres = pg2;
            return true;
        }

        if (ex.InnerException is PostgresException pg3)
        {
            postgres = pg3;
            return true;
        }

        return false;
    }
}

public sealed record ApiResponse(
    int Status,
    string Title,
    string Data,
    string TraceId,
    IReadOnlyList<ApiError> Errors);

public sealed record ApiError(
    string Code,
    string Message,
    string? Field);