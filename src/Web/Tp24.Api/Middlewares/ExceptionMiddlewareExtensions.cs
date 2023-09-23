using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Tp24.Common.Wrappers;

namespace Tp24.Api.Middlewares;

public static class ExceptionMiddlewareExtensions
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public static void UseTp24ExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(err =>
        {
            err.Run(async ctx =>
            {
                var exception = ctx.Features.Get<IExceptionHandlerFeature>();
                ctx.Response.ContentType = "application/json";

                if (exception != null)
                {
                    string resultMessage;

                    switch (exception.Error)
                    {
                        default:
                            ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            resultMessage = "An unexpected error occurred. Please try again later.";
                            break;
                    }

                    var response = await Result.FailAsync(resultMessage);
                    await ctx.Response.WriteAsync(JsonSerializer.Serialize(response, SerializerOptions));
                }
            });
        });
    }
}