using Tp24.Infrastructure.DataAccess;

namespace Tp24.IntegrationTest.TestData.Interfaces;

public interface ITestDataSeed
{
    public int Order { get; }
    Task SeedAsync(Tp24DbContext context);
}