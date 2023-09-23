using Microsoft.Extensions.DependencyInjection;
using Tp24.Infrastructure.DataAccess.Seed.Interfaces;
using Tp24.Infrastructure.DataAccess.Seed.Profiles;

namespace Tp24.Infrastructure.DataAccess.Seed.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection RegisterDatabaseSeeders(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseSeed, DebtorDatabaseSeed>();
        services.AddTransient<IDatabaseSeed, ReceivableDatabaseSeed>();

        return services;
    }
}