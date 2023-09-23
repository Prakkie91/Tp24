using Tp24.Application.Features.Receivable.Commands;
using Tp24.Core.Domain.Entities;
using Tp24.Infrastructure.DataAccess.Entities;

namespace Tp24.IntegrationTest.Factories;

public static class ReceivableTestDataFactory
{
    public static AddReceivablesCommand CreateAddReceivablesCommand(
        string reference = "TestReference",
        string currencyCode = "USD",
        DateTime? issueDate = null,
        decimal openingValue = 1000m,
        decimal paidValue = 500m,
        DateTime? dueDate = null,
        string debtorName = "Test Debtor",
        string debtorReference = "TestDebtorRef",
        string debtorCountryCode = "US"
    )
    {
        issueDate ??= DateTime.Now;
        dueDate ??= DateTime.Now.AddDays(30);

        return new AddReceivablesCommand
        {
            Reference = reference,
            CurrencyCode = currencyCode,
            IssueDate = issueDate.Value,
            OpeningValue = openingValue,
            PaidValue = paidValue,
            DueDate = dueDate.Value,
            DebtorName = debtorName,
            DebtorReference = debtorReference,
            DebtorCountryCode = debtorCountryCode
        };
    }

    public static AddReceivablesBatchCommand CreateAddReceivablesBatchCommand(int numReceivables = 3)
    {
        return new AddReceivablesBatchCommand
        {
            Receivables = CreateListOfReceivableDtos(numReceivables)
        };
    }

    public static List<ReceivableDto> CreateListOfReceivableDtos(int numReceivables = 3)
    {
        var receivableDtos = new List<ReceivableDto>();
        for (int i = 0; i < numReceivables; i++)
        {
            receivableDtos.Add(CreateReceivableDto($"Ref{i}", $"USD", DateTime.Now, 1000m + i, 500m + i, DateTime.Now.AddDays(30 + i)));
        }
        return receivableDtos;
    }

    public static ReceivableDto CreateReceivableDto(
        string reference = "TestReference",
        string currencyCode = "USD",
        DateTime issueDate = default,
        decimal openingValue = 1000m,
        decimal paidValue = 500m,
        DateTime dueDate = default,
        string debtorName = "Test Debtor",
        string debtorReference = "TestDebtorRef",
        string debtorCountryCode = "US"
    )
    {
        if (issueDate == default)
            issueDate = DateTime.Now;

        if (dueDate == default)
            dueDate = DateTime.Now.AddDays(30);

        return new ReceivableDto
        {
            Reference = reference,
            CurrencyCode = currencyCode,
            IssueDate = issueDate,
            OpeningValue = openingValue,
            PaidValue = paidValue,
            DueDate = dueDate,
            DebtorName = debtorName,
            DebtorReference = debtorReference,
            DebtorCountryCode = debtorCountryCode
        };
    }
}