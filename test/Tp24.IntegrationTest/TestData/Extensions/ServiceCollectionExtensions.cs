using Microsoft.Extensions.DependencyInjection;
using Tp24.Infrastructure.DataAccess.Seed.Profiles;
using Tp24.IntegrationTest.TestData.Interfaces;
using Tp24.IntegrationTest.TestData.Profiles;

namespace Tp24.IntegrationTest.TestData.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection RegisterTestDataSeeders(this IServiceCollection services)
    {
        services.AddTransient<ITestDataSeed, ReceivableTestDataSeed>();
        services.AddTransient<ITestDataSeed, DebtorTestDataSeed>();

        return services;
    }
}