namespace Tp24.Core.Domain.Entities;

public class ReceivablesSummaryDomainModel
{
    public int TotalReceivables { get; set; }
    public int OpenInvoiceCount { get; set; }
    public int ClosedInvoiceCount { get; set; }
    public decimal TotalOpeningValue { get; set; }
    public decimal TotalPaidValue { get; set; }
    public int NumOfOverdueInvoices { get; set; }
    public decimal TotalOverdueAmount { get; set; }
    public int UniqueDebtors { get; set; }
}