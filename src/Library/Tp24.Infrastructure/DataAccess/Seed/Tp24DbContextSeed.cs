using Microsoft.Extensions.DependencyInjection;
using Tp24.Infrastructure.DataAccess.Seed.Interfaces;

namespace Tp24.Infrastructure.DataAccess.Seed;

public static class Tp24DbContextSeed
{
    public static async Task SeedAsync(this Tp24DbContext context, IServiceProvider serviceProvider)
    {
        // Retrieve all seeders from the service collection
        var seeders = serviceProvider.GetServices<IDatabaseSeed>()
            .OrderBy(s => s.Order)
            .ToList();

        foreach (var seeder in seeders) await seeder.SeedAsync(context);
    }
}