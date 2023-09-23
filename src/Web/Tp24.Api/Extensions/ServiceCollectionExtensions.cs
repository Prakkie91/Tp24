using Microsoft.OpenApi.Models;

namespace Tp24.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static void RegisterSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            // include all project's xml comments
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                if (!assembly.IsDynamic)
                {
                    var xmlFile = $"{assembly.GetName().Name}.xml";
                    var xmlPath = Path.Combine(baseDirectory, xmlFile);
                    if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
                }

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Tp24 API"
            });
        });
    }
}