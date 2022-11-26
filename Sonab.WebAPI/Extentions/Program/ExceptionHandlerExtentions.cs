using System.Text.Json;
using Sonab.WebAPI.Models;

namespace Sonab.WebAPI.Extentions.Program;

public static class ExceptionHandlerExtentions
{
    public static IApplicationBuilder UseSonabExceptionHandler(this IApplicationBuilder app) => app.UseExceptionHandler(builder =>
    {
        builder.Run(async context =>
        {
            // "Magic" number of status code:)
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            ErrorMessages message = new("Server error", "Unexpected server error. Please contact dev team");
            await context.Response.WriteAsync(JsonSerializer.Serialize(message));
        });
    });
}
