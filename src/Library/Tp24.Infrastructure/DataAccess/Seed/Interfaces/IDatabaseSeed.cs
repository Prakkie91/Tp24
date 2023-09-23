namespace Tp24.Infrastructure.DataAccess.Seed.Interfaces;

public interface IDatabaseSeed
{
    public int Order { get; }
    Task SeedAsync(Tp24DbContext context);
}