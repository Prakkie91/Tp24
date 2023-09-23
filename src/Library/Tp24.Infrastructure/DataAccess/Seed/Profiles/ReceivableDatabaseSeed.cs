using Tp24.Infrastructure.DataAccess.Entities;
using Tp24.Infrastructure.DataAccess.Seed.Interfaces;

namespace Tp24.Infrastructure.DataAccess.Seed.Profiles;

public class ReceivableDatabaseSeed : IDatabaseSeed
{
    private static readonly Random Random = new();

    public int Order => 2;

    public async Task SeedAsync(Tp24DbContext context)
    {
        // Receivables for John Doe Corp
        var receivable1 = new ReceivableDataModel
        {
            Id = ReceivableIds.Receivable1,
            Reference = "JD-R001",
            CurrencyCode = "USD",
            IssueDate = new DateTime(2023, 1, 1),
            OpeningValue = 1000.00M,
            PaidValue = 500.00M,
            DueDate = new DateTime(2023, 2, 1),
            DebtorId = DebtorDataModelSeed.DebtorIds.JohnDoeCorp
        };

        // Receivables for Smith Inc
        var receivable2 = new ReceivableDataModel
        {
            Id = ReceivableIds.Receivable2,
            Reference = "SI-R002",
            CurrencyCode = "USD",
            IssueDate = new DateTime(2023, 1, 15),
            OpeningValue = 2000.00M,
            PaidValue = 1000.00M,
            DueDate = new DateTime(2023, 2, 15),
            DebtorId = DebtorDataModelSeed.DebtorIds.SmithInc
        };

        var receivables = GenerateRandomReceivables(200);

        receivables.Add(receivable1);
        receivables.Add(receivable2);
        await context.Receivables.AddRangeAsync(receivables);
        await context.SaveChangesAsync();
    }

    private static List<ReceivableDataModel> GenerateRandomReceivables(int count)
    {
        var receivables = new List<ReceivableDataModel>();

        for (var i = 0; i < count; i++)
        {
            var issueDate = RandomDate(new DateTime(2023, 1, 1), new DateTime(2023, 3, 1));
            var openingValue = RandomDecimal(500.00M, 5000.00M);
            var paidValue = openingValue - RandomDecimal(0, openingValue / 2);

            receivables.Add(new ReceivableDataModel
            {
                Id = Guid.NewGuid(),
                Reference = $"AUTO-R{100 + i}",
                CurrencyCode = "USD",
                IssueDate = issueDate,
                OpeningValue = openingValue,
                PaidValue = paidValue,
                DueDate = issueDate.AddMonths(1),
                DebtorId = i % 2 == 0
                    ? DebtorDataModelSeed.DebtorIds.JohnDoeCorp
                    : DebtorDataModelSeed.DebtorIds.SmithInc
            });
        }

        return receivables;
    }

    private static DateTime RandomDate(DateTime start, DateTime end)
    {
        var range = (end - start).Days;
        return start.AddDays(Random.Next(range));
    }

    private static decimal RandomDecimal(decimal min, decimal max)
    {
        var range = (double)(max - min);
        return min + (decimal)(Random.NextDouble() * range);
    }

    public static class ReceivableIds
    {
        public static readonly Guid Receivable1 = new("8D8F9F9A-BCDA-4BFC-9ABD-02E2D8C9E234");
        public static readonly Guid Receivable2 = new("3A2D6BCD-ED56-4D67-8F9D-6E7E8A9B3DEF");
    }
}