using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Tp24.Api.Extensions;
using Tp24.Api.Middlewares;
using Tp24.Application;
using Tp24.Common.Wrappers;
using Tp24.Infrastructure;
using Tp24.Infrastructure.DataAccess;
using Tp24.Infrastructure.DataAccess.Seed;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure();
    builder.Services.AddControllers()
        .AddFluentValidation()
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                var result = new Result
                {
                    Succeeded = false,
                    Messages = errors
                };

                return new BadRequestObjectResult(result);
            };
        });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.RegisterSwagger();
}
var app = builder.Build();

app.UseTp24ExceptionHandler();

if (app.Environment.IsDevelopment())
{
    var mapper = app.Services.GetRequiredService<IMapper>();
    mapper.ConfigurationProvider.AssertConfigurationIsValid();
    app.ConfigureSwagger();
}

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var context = scopedProvider.GetRequiredService<Tp24DbContext>();
        if (app.Environment.IsDevelopment()) await context.SeedAsync(scopedProvider);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

namespace Tp24.Api
{
    public class Program
    {
    }
}