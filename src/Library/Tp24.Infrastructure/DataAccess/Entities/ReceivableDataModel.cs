using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tp24.Infrastructure.DataAccess.Entities.Common;

namespace Tp24.Infrastructure.DataAccess.Entities;

/// <summary>
///     Represents a receivable entity, detailing debts owed to a company for provided services or goods.
/// </summary>
[Table("Receivable")]
public class ReceivableDataModel : BaseEntity
{
    /// <summary>
    ///     Gets or sets the reference number or code for the receivable.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Reference { get; set; }

    /// <summary>
    ///     Gets or sets the currency code for the receivable amount.
    /// </summary>
    [Required]
    [MaxLength(3)]
    public string CurrencyCode { get; set; }

    /// <summary>
    ///     Gets or sets the date when the receivable was issued.
    /// </summary>
    [Required]
    public DateTime IssueDate { get; set; }

    /// <summary>
    ///     Gets or sets the initial value or amount of the receivable.
    /// </summary>
    [Required]
    public decimal OpeningValue { get; set; }

    /// <summary>
    ///     Gets or sets the value or amount of the receivable that has been paid.
    /// </summary>
    [Required]
    public decimal PaidValue { get; set; }

    /// <summary>
    ///     Gets or sets the due date by which the receivable should be settled.
    /// </summary>
    [Required]
    public DateTime DueDate { get; set; }

    /// <summary>
    ///     Gets or sets the date when the receivable was closed or settled.
    /// </summary>
    public DateTime? ClosedDate { get; set; }

    /// <summary>
    ///     Gets or sets a boolean indicating whether the receivable was cancelled.
    /// </summary>
    public bool Cancelled { get; set; }

    /// <summary>
    ///     Gets or sets the foreign key reference to the associated debtor's ID.
    /// </summary>
    [Required]
    [ForeignKey(nameof(DebtorDataModel))]
    public Guid DebtorId { get; set; }

    /// <summary>
    ///     Navigation property for the associated debtor of the receivable.
    /// </summary>
    public virtual DebtorDataModel Debtor { get; set; }
}