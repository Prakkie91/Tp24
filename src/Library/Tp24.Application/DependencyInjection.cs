using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Tp24.Application.Features.Receivable.Commands;
using Tp24.Application.Features.Receivable.Validators;

namespace Tp24.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<ReceivableDtoValidator>();
        return services.AddMediatR(typeof(AddReceivablesCommand).Assembly);
    }
}