using Tp24.Application.Features.Receivable.Commands;
using Tp24.Core.Domain.Entities;
using Tp24.Infrastructure.DataAccess.Entities;

namespace Tp24.UnitTest.Factories;

public static class ReceivableTestDataFactory
{
    public static AddReceivablesCommand CreateAddReceivablesCommand()
    {
        return new AddReceivablesCommand
        {
            Reference = "TestReference",
            CurrencyCode = "USD",
            IssueDate = DateTime.Now,
            OpeningValue = 1000m,
            PaidValue = 500m,
            DueDate = DateTime.Now.AddDays(30),
            DebtorName = "Test Debtor",
            DebtorReference = "TestDebtorRef",
            DebtorCountryCode = "US"
        };
    }

    public static ReceivablesSummaryDomainModel CreateReceivablesSummary()
    {
        return new ReceivablesSummaryDomainModel
        {
            TotalReceivables = 10,
            OpenInvoiceCount = 5,
            ClosedInvoiceCount = 5,
            TotalOpeningValue = 1000.00M,
            TotalPaidValue = 500.00M,
            NumOfOverdueInvoices = 2,
            TotalOverdueAmount = 300.00M,
            UniqueDebtors = 8
        };
    }

    public static ReceivableDomainModel CreateReceivableDomainModel(
        string reference = "RCV001",
        string currencyCode = "USD",
        DateTime? issueDate = null,
        decimal openingValue = 1000.00M,
        decimal paidValue = 500.00M,
        DateTime? dueDate = null,
        DebtorDomainModel? debtor = null
    )
    {
        issueDate ??= DateTime.UtcNow;
        dueDate ??= DateTime.UtcNow.AddMonths(1);
        debtor ??= DebtorTestDataFactory.CreateDebtorDomainModel();

        return new ReceivableDomainModel(reference, currencyCode, issueDate.Value, openingValue,
            paidValue, dueDate.Value, debtor);
    }

    public static ReceivableDataModel CreateReceivableDataModel(
        string reference = "RCV001",
        string currencyCode = "USD",
        DateTime? issueDate = null,
        decimal openingValue = 1000.00M,
        decimal paidValue = 500.00M,
        DateTime? dueDate = null,
        DebtorDataModel? debtor = null
    )
    {
        issueDate ??= DateTime.UtcNow;
        dueDate ??= DateTime.UtcNow.AddMonths(1);
        debtor ??= DebtorTestDataFactory.CreateDebtorDataModel();

        return new ReceivableDataModel
        {
            Reference = reference,
            CurrencyCode = currencyCode,
            IssueDate = issueDate.Value,
            OpeningValue = openingValue,
            PaidValue = paidValue,
            DueDate = dueDate.Value,
            DebtorId = debtor.Id,
            Debtor = debtor
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