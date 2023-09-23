using Tp24.Infrastructure.DataAccess.Entities;
using Tp24.Infrastructure.DataAccess.Seed.Interfaces;

namespace Tp24.Infrastructure.DataAccess.Seed.Profiles;

public class DebtorDatabaseSeed : IDatabaseSeed
{
    public int Order => 2;

    public async Task SeedAsync(Tp24DbContext context)
    {
        // Debtors for USA
        var johnDoeCorp = new DebtorDataModel
        {
            Id = DebtorIds.JohnDoeCorp,
            Name = "John Doe Corp",
            Reference = "JD-001",
            CountryCode = "USA"
        };
        var smithInc = new DebtorDataModel
        {
            Id = DebtorIds.SmithInc,
            Name = "Smith Inc",
            Reference = "SI-002",
            CountryCode = "USA"
        };

        // Debtors for Sweden
        var svenskaCorp = new DebtorDataModel
        {
            Id = DebtorIds.SvenskaCorp,
            Name = "Svenska Corp",
            Reference = "SV-003",
            CountryCode = "SE"
        };

        await context.Debtors.AddRangeAsync(johnDoeCorp, smithInc, svenskaCorp);

        await context.SaveChangesAsync();
    }

    public static class DebtorIds
    {
        // Debtors for USA
        public static readonly Guid JohnDoeCorp = new("01234567-0123-0123-0123-0123456789AB");
        public static readonly Guid SmithInc = new("12345678-1234-1234-1234-1234567890BC");

        // Debtors for Sweden
        public static readonly Guid SvenskaCorp = new("23456789-2345-2345-2345-2345678901CD");
    }
}