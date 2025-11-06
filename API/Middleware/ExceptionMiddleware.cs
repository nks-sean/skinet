using System;
using System.Text.Json;

namespace API.Middleware;

public class ExceptionMiddleware(IHostEnvironment env, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex, context, env);
        }
    }

    private static Task HandleExceptionAsync(Exception ex, HttpContext context, IHostEnvironment env)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = env.IsDevelopment()
                ? new ApiErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ApiErrorResponse(context.Response.StatusCode, "Internal Server Error");

         var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

         var json = JsonSerializer.Serialize(response, options);

        return context.Response.WriteAsync(json);
    }
}

