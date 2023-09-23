using MediatR;
using Tp24.Common.Wrappers;

namespace Tp24.Application.Features.Receivable.Commands;

public class AddReceivablesBatchCommand : IRequest<IResult>
{
    public List<ReceivableDto> Receivables { get; set; }
}

public class ReceivableDto
{
    public string Reference { get; set; }
    public string CurrencyCode { get; set; }
    public DateTime IssueDate { get; set; }
    public decimal OpeningValue { get; set; }
    public decimal PaidValue { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public bool? Cancelled { get; set; }
    public string DebtorName { get; set; }
    public string DebtorReference { get; set; }
    public string DebtorAddress1 { get; set; }
    public string DebtorAddress2 { get; set; }
    public string DebtorTown { get; set; }
    public string DebtorState { get; set; }
    public string DebtorZip { get; set; }
    public string DebtorCountryCode { get; set; }
    public string DebtorRegistrationNumber { get; set; }
}