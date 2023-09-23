using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tp24.Infrastructure.DataAccess;

namespace Tp24.UnitTest;

public class InMemoryDatabaseHelper
{
    public static DbContextOptions<Tp24DbContext> CreateNewContextOptions()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .AddEntityFrameworkProxies()
            .BuildServiceProvider();
        var builder = new DbContextOptionsBuilder<Tp24DbContext>();
        builder.UseInMemoryDatabase("TestTp24Db")
            .UseInternalServiceProvider(serviceProvider);
        return builder.Options;
    }
}