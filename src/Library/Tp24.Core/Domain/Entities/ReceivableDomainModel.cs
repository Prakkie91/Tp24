using Tp24.Core.Domain.Entities.Common;

namespace Tp24.Core.Domain.Entities;

public class ReceivableDomainModel : BaseDomainModel
{
    public ReceivableDomainModel()
    {
    }

    public ReceivableDomainModel(string reference, string currencyCode, DateTime issueDate, decimal openingValue,
        decimal paidValue, DateTime dueDate, DebtorDomainModel debtor)
    {
        SetReference(reference);
        SetCurrencyCode(currencyCode);
        SetIssueDate(issueDate);
        SetOpeningValue(openingValue);
        SetPaidValue(paidValue);
        SetDueDate(dueDate);
        SetDebtor(debtor);
    }

    public string Reference { get; private set; }
    public string CurrencyCode { get; private set; }
    public DateTime IssueDate { get; private set; }
    public decimal OpeningValue { get; private set; }
    public decimal PaidValue { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? ClosedDate { get; }
    public bool Cancelled { get; }
    public DebtorDomainModel Debtor { get; private set; }
    public Guid DebtorId { get; private set; }

    private void SetReference(string reference)
    {
        if (string.IsNullOrWhiteSpace(reference) || reference.Length > 255)
            throw new ArgumentException("Invalid reference.", nameof(reference));

        Reference = reference;
    }

    private void SetCurrencyCode(string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode) || currencyCode.Length != 3)
            throw new ArgumentException("Invalid currency code.", nameof(currencyCode));

        CurrencyCode = currencyCode;
    }

    private void SetIssueDate(DateTime issueDate)
    {
        IssueDate = issueDate;
    }

    private void SetOpeningValue(decimal openingValue)
    {
        OpeningValue = openingValue;
    }

    private void SetPaidValue(decimal paidValue)
    {
        PaidValue = paidValue;
    }

    private void SetDueDate(DateTime dueDate)
    {
        DueDate = dueDate;
    }

    private void SetDebtor(DebtorDomainModel debtor)
    {
        Debtor = debtor ?? throw new ArgumentNullException(nameof(debtor));
        DebtorId = debtor.Id;
    }
}