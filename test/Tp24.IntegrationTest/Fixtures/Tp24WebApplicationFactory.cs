using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tp24.Infrastructure.DataAccess;
using Tp24.IntegrationTest.TestData;
using Tp24.IntegrationTest.TestData.Extensions;

namespace Tp24.IntegrationTest.Fixtures;

public class Tp24WebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<Tp24DbContext>));
            if (descriptor != null) services.Remove(descriptor);

            services.AddDbContext<Tp24DbContext>(options =>
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });

            services.RegisterTestDataSeeders();
            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<Tp24DbContext>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                db.SeedTestDataAsync(scope.ServiceProvider).GetAwaiter().GetResult();
            }
        });

        builder.UseEnvironment("Testing");
    }
}