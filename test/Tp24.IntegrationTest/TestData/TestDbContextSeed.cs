using Microsoft.Extensions.DependencyInjection;
using Tp24.Infrastructure.DataAccess;
using Tp24.IntegrationTest.TestData.Interfaces;

namespace Tp24.IntegrationTest.TestData;

public static class TestDbContextSeed
{
    public static async Task SeedTestDataAsync(this Tp24DbContext context, IServiceProvider serviceProvider)
    {
        // Retrieve all seeders from the service collection
        var seeders = serviceProvider.GetServices<ITestDataSeed>().OrderBy(s => s.Order).ToList();

        foreach (var seeder in seeders) await seeder.SeedAsync(context);
    }
}