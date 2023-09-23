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
                    List<string> errors = null;

                    switch (ctx.Response.StatusCode)
                    {
                        case (int)HttpStatusCode.BadRequest:
                            var errorResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(await new StreamReader(ctx.Response.Body).ReadToEndAsync());
                            if (errorResponse.ContainsKey("errors"))
                            {
                                var errorDetails = errorResponse["errors"] as Dictionary<string, object>;
                                errors = new List<string>();
                                foreach (var error in errorDetails)
                                {
                                    errors.AddRange(((JsonElement)error.Value).EnumerateArray().Select(e => e.GetString()));
                                }
                            }
                            resultMessage = "Validation failed";
                            break;

                        default:
                            resultMessage = "An unexpected error occurred. Please try again later.";
                            break;
                    }

                    var response = errors != null ? await Result.FailAsync(errors) : await Result.FailAsync(resultMessage);
                    await ctx.Response.WriteAsync(JsonSerializer.Serialize(response, SerializerOptions));
                }
            });
        });
    }
}
