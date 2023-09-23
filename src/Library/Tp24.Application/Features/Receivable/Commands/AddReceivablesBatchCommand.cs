using MediatR;
using Tp24.Application.Features.Receivable.Responses;
using Tp24.Common.Wrappers;

namespace Tp24.Application.Features.Receivable.Commands;

/// <summary>
///     Represents a command to add a batch of receivables to the system.
/// </summary>
public class AddReceivablesBatchCommand : IRequest<IResult<AddReceivablesBatchResponse>>
{
    /// <summary>
    ///     Gets or sets the list of receivables to be processed.
    /// </summary>
    public List<ReceivableDto> Receivables { get; set; }
}

/// <summary>
///     Data Transfer Object (DTO) representing the details of a single receivable.
/// </summary>
public class ReceivableDto
{
    /// <summary>
    ///     Gets or sets the unique reference for the receivable.
    /// </summary>
    public string Reference { get; set; }

    /// <summary>
    ///     Gets or sets the currency code associated with the receivable amount.
    /// </summary>
    public string CurrencyCode { get; set; }

    /// <summary>
    ///     Gets or sets the issue date of the receivable.
    /// </summary>
    public DateTime IssueDate { get; set; }

    /// <summary>
    ///     Gets or sets the opening value or original amount of the receivable.
    /// </summary>
    public decimal OpeningValue { get; set; }

    /// <summary>
    ///     Gets or sets the value of the receivable that has been paid.
    /// </summary>
    public decimal PaidValue { get; set; }

    /// <summary>
    ///     Gets or sets the due date by which the receivable amount should be paid.
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    ///     Gets or sets the date on which the receivable was closed or settled.
    ///     It will be null if the receivable is still open.
    /// </summary>
    public DateTime? ClosedDate { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the receivable is cancelled.
    /// </summary>
    public bool? Cancelled { get; set; }

    /// <summary>
    ///     Gets or sets the name of the debtor associated with the receivable.
    /// </summary>
    public string DebtorName { get; set; }

    /// <summary>
    ///     Gets or sets the reference identifier for the debtor.
    /// </summary>
    public string DebtorReference { get; set; }

    /// <summary>
    ///     Gets or sets the first line of the debtor's address.
    /// </summary>
    public string? DebtorAddress1 { get; set; }

    /// <summary>
    ///     Gets or sets the second line of the debtor's address.
    /// </summary>
    public string? DebtorAddress2 { get; set; }

    /// <summary>
    ///     Gets or sets the town or city where the debtor is located.
    /// </summary>
    public string? DebtorTown { get; set; }

    /// <summary>
    ///     Gets or sets the state or province of the debtor's address.
    /// </summary>
    public string? DebtorState { get; set; }

    /// <summary>
    ///     Gets or sets the postal or ZIP code of the debtor's address.
    /// </summary>
    public string? DebtorZip { get; set; }

    /// <summary>
    ///     Gets or sets the country code where the debtor is located.
    /// </summary>
    public string? DebtorCountryCode { get; set; }

    /// <summary>
    ///     Gets or sets the registration number of the debtor, if applicable.
    /// </summary>
    public string? DebtorRegistrationNumber { get; set; }
}
