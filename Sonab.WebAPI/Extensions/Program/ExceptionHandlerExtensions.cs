using System.Text.Json;
using Sonab.WebAPI.Models;

namespace Sonab.WebAPI.Extensions.Program;

public static class ExceptionHandlerExtensions
{
    public static IApplicationBuilder UseSonabExceptionHandler(this IApplicationBuilder app) => app.UseExceptionHandler(builder =>
    {
        builder.Run(async context =>
        {
            // "Magic" number of status code:)
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            ErrorMessages message = new("Server error", "Error.Server");
            await context.Response.WriteAsync(JsonSerializer.Serialize(message));
        });
    });
}
